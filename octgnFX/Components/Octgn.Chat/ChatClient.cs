using Common.Logging;
using MassTransit;
using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using System.Threading.Tasks;

namespace Octgn.Chat
{
    public class ChatClient
    {
        private static ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string Username { get; private set; }
        public string Password {
            get {
                IntPtr unmanagedString = IntPtr.Zero;
                try {
                    unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(_password);
                    return Marshal.PtrToStringUni(unmanagedString);
                } finally {
                    Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
                }
            }
            set {
                _password.Clear();
                foreach (char c in value?.ToCharArray()) {
                    _password.AppendChar(c);
                }
            }
        }

        public TimeSpan MaxRpcTimeout { get; set; } = TimeSpan.FromSeconds(10);

        private string _sessionId;

        private readonly IBusControl _bus;
        private readonly Uri _rabbitAddress;
        private readonly MessageRequestClient<Handshake, HandshakeResponse> _handshakeRpc;
        private readonly MessageRequestClient<JoinRoom, JoinRoomResponse> _joinRoomRpc;
        private readonly SecureString _password = new SecureString();

        public ChatClient(string username, string password) {
            _rabbitAddress = new Uri("rabbitmq://octgn.local/chat");
            _bus = ConfigureBus(Username = username, Password = password);
            _handshakeRpc = new MessageRequestClient<Handshake, HandshakeResponse>(_bus, new Uri("rabbitmq://octgn.local/chat/user_send_queue"), MaxRpcTimeout);
            _joinRoomRpc = new MessageRequestClient<JoinRoom, JoinRoomResponse>(_bus, new Uri("rabbitmq://octgn.local/chat/user_send_queue"), MaxRpcTimeout);
        }

        public async Task Start(CancellationToken cancel = default(CancellationToken)) {
            await _bus.StartAsync(cancel);
            var result = await _handshakeRpc.Request(new Handshake {
                Username = Username,
                Password = Password
            }, cancel);
            _sessionId = result?.SessionId;
        }

        public async Task Stop() {
            await _bus.StopAsync();
        }

        IBusControl ConfigureBus(string username, string password) {
            return Bus.Factory.CreateUsingRabbitMq(cfg => {
                var host = cfg.Host(_rabbitAddress, h => {
                    h.Username(username);
                    h.Password(password);
                });
                cfg.ReceiveEndpoint(host, $"user_{username}_receive_queue", e => {
                    e.Handler<GroupMessage>(HandleOnGroupMessage);
                    e.Handler<Message>(HandleOnMessage);
                });
            });
        }

        public async Task Send(string to, string message) {
            var msg = new Message {
                To = to,
                MessageText = message,
                DateSent = DateTimeOffset.Now,
                SessionId = _sessionId,
            };
            ISendEndpoint endpoint = await _bus.GetSendEndpoint(new Uri("rabbitmq://octgn.local/chat/user_send_queue"));
            await endpoint.Send(msg);
        }

        public async Task SendToRoom(string room, string message) {
            var msg = new GroupMessage {
                Room = room,
                MessageText = message,
                DateSent = DateTimeOffset.Now,
                SessionId = _sessionId,
            };
            ISendEndpoint endpoint = await _bus.GetSendEndpoint(new Uri("rabbitmq://octgn.local/chat/user_send_queue"));
            await endpoint.Send(msg);
        }

        public async Task<Room> JoinRoom(string name) {
            for (var count = 0; count < 3; count++) {
                var result = await _joinRoomRpc.Request(new JoinRoom {
                    SessionId = _sessionId,
                    RoomName = name
                }, CancellationToken.None);
                switch (result.Result) {
                    case JoinRoomResponse.ResultType.Ok:
                    var room = Room.Get(result.Room.Name, result.Room);
                    room.ChatClient = this;
                    return room;

                    case JoinRoomResponse.ResultType.ErrorTryAgain:
                    await Task.Delay(5000);
                    break;

                    case JoinRoomResponse.ResultType.Unauthorized:
                    throw new UnauthorizedException();

                    default:
                    throw new NotImplementedException(result.Result.ToString());
                }
            }
            return null;
        }

        protected virtual async Task HandleOnMessage(ConsumeContext<Message> context) {
            var message = context.Message;
            if (message is GroupMessage) throw new InvalidOperationException();
            OnMessage?.Invoke(this, new OnMessageEventArgs(message));
        }

        protected virtual async Task HandleOnGroupMessage(ConsumeContext<GroupMessage> context) {
            var message = context.Message;
            var room = Room.Get(message.Room);
            room.ProcessMessage(message);
        }

        public event EventHandler<OnMessageEventArgs> OnMessage;
        public class OnMessageEventArgs : EventArgs
        {
            public Message Message { get; set; }

            public OnMessageEventArgs() {

            }

            public OnMessageEventArgs(Message message) {
                Message = message;
            }
        }
    }

    public class UnauthorizedException : Exception
    {

    }
}
