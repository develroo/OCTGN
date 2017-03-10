using Common.Logging;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Octgn.Chat
{
    public class ChatClient
    {
        private static ILog Log = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        public string Username { get; private set; }

        private readonly IBusControl _bus;

        public ChatClient(string username, string password) {
            _bus = ConfigureBus(Username = username, password);
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
                    h.Username( username );
                    h.Password( password );
                } );
                //cfg.ReceiveEndpoint( host, username + "_receive_queue", e => {
                //    e.Handler<Message>( async context => {
                //        context.Message.From = context.SourceAddress.GetHashCode().ToString();
                //        await OnMessage( context.Message );
                //    } );
                //} );
            } );
        }

        public async void Send( Message message ) {
            message.DateSent = DateTimeOffset.Now;
            ISendEndpoint endpoint = await _bus.GetSendEndpoint( new Uri("rabbitmq://octgn.local/chat/user_send_queue") );
            await endpoint.Send( message );
        }

        protected virtual async Task OnMessage( Message message ) {
            await Console.Out.WriteLineAsync( $"{message.From}: {message.MessageText}" );
        }
    }
}
