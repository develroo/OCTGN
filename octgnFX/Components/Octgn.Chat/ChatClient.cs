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
        private readonly SecureString _password = new SecureString();

        public ChatClient(string username, string password) {
            _rabbitAddress = new Uri("rabbitmq://octgn.local/chat");
            _bus = ConfigureBus(Username = username, Password = password);
            _handshakeRpc = new MessageRequestClient<Handshake, HandshakeResponse>(_bus, new Uri("rabbitmq://octgn.local/chat/user_send_queue"), MaxRpcTimeout);
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
                    e.Handler<Message>(async context => {
                        await OnMessage(context.Message);
                    });
                });
            });
        }

        public async void Send(string to, string message) {
            var msg = new Message {
                To = to,
                MessageText = message,
                DateSent = DateTimeOffset.Now,
                SessionId = _sessionId,
            };
            ISendEndpoint endpoint = await _bus.GetSendEndpoint(new Uri("rabbitmq://octgn.local/chat/user_send_queue"));
            await endpoint.Send(msg);
        }

        public async Task<Room> JoinRoom(string name) {
            throw new NotImplementedException("Dylan says he's ready");
        }

        protected virtual async Task OnMessage(Message message) {
            await Console.Out.WriteLineAsync($"{message.From}: {message.MessageText}");
        }
    }
}
