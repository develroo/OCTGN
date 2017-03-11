using Common.Logging;
using MassTransit;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Octgn.Chat
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
                    e.Handler<Message>(OnMessage);
                    e.Handler<Handshake>(OnHandshake);
                } );
             } );
        }

        protected virtual async Task OnMessage( ConsumeContext<Message> context ) {
            var message = context.Message;
            string username = message.SessionId;
            message.From = username;

            var se = await context.GetSendEndpoint(new Uri($"rabbitmq://octgn.local/chat/user_{message.To}_receive_queue"));
            message.SessionId = null;
            await se.Send(message);
        }

        protected virtual async Task OnHandshake( ConsumeContext<Handshake> context ) {
            var resp = new HandshakeResponse {
                SessionId = context.Message.Username
            };
            await context.RespondAsync(resp);
        }
    }
}
