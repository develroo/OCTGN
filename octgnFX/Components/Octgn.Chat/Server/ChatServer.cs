using Common.Logging;
using MassTransit;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Octgn.Chat.Server
{
    public class ChatServer
    {
        private static int _id = 0;
        private int Id { get; } = Interlocked.Increment(ref _id);


        private static ILog Log = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        private readonly IBusControl _bus;

        public ChatServer(string username, string password) {
            _bus = ConfigureBus(username, password);
        }

        public void Start() {
            _bus.Start();
        }

        public void Stop() {
            _bus?.Stop( TimeSpan.FromSeconds( 30 ) );
        }

        IBusControl ConfigureBus(string username, string password) {
            return Bus.Factory.CreateUsingRabbitMq( cfg => {
                var host = cfg.Host( new Uri( "rabbitmq://octgn.local/chat" ), h => {
                    h.Username(username);
                    h.Password(password);
                } );
                cfg.ReceiveEndpoint( host, "user_send_queue", e => {
                    e.Handler<GroupMessage>(OnGroupMessage);
                    e.Handler<Message>(OnMessage);
                    e.Handler<Handshake>(OnHandshake);
                    e.Handler<JoinRoom>(OnJoinRoom);
                } );
             } );
        }

        protected virtual async Task OnMessage( ConsumeContext<Message> context ) {
            var message = context.Message;
            message.From = UserFromSessionId(message.SessionId);

            var se = await context.GetSendEndpoint(new Uri($"rabbitmq://octgn.local/chat/user_{message.To}_receive_queue"));
            message.SessionId = null;
            await se.Send(message);
        }

        protected virtual async Task OnGroupMessage( ConsumeContext<GroupMessage> context ) {
            var message = context.Message;
            message.From = UserFromSessionId(message.SessionId);

            var room = Room.Get(message.Room);
            if (room == null) throw new RoomNotFoundException(message.Room);

            foreach (var user in room.Users) {
                var se = await context.GetSendEndpoint(new Uri($"rabbitmq://octgn.local/chat/user_{user}_receive_queue"));
                message.SessionId = null;
                await se.Send(message);
            }
        }

        protected virtual async Task OnHandshake( ConsumeContext<Handshake> context ) {
            var resp = new HandshakeResponse {
                SessionId = context.Message.Username
            };
            await context.RespondAsync(resp);
        }

        protected virtual async Task OnJoinRoom( ConsumeContext<JoinRoom> context) {
            var resp = new JoinRoomResponse {
                Room = Room.Join(context.Message.SessionId, context.Message.RoomName)
            };
            if(resp.Room == null) {
                resp.Result = JoinRoomResponse.ResultType.ErrorTryAgain;
            } else {
                resp.Result = JoinRoomResponse.ResultType.Ok;
            }

            await context.RespondAsync(resp);
        }

        private string UserFromSessionId(string sessionId) {
            return sessionId;
        }
    }

    public class RoomNotFoundException : Exception
    {
        public RoomNotFoundException() : base() {

        }

        public RoomNotFoundException(string roomName) : base($"Room {roomName} not found.") {
}
    }
}
