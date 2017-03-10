﻿using Common.Logging;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Octgn.Chat
{
    public class ChatServer
    {
        private static ILog Log = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        private readonly IBusControl _bus;

        public ChatServer() {
            _bus = ConfigureBus();
        }

        public void Start() {
            _bus.Start();
        }

        public void Stop() {
            _bus?.Stop( TimeSpan.FromSeconds( 30 ) );
        }

        IBusControl ConfigureBus() {
            return Bus.Factory.CreateUsingRabbitMq( cfg => {
                var host = cfg.Host( new Uri( "rabbitmq://octgn.local" ), h => {
                    h.Username( "" );
                    h.Password( "" );
                } );
                cfg.ReceiveEndpoint( host, "event_queue", e => {
                    e.Handler<Message>( async context => {
                        await OnMessage( context.Message );
                    } );
                } );
             } );
        }

        protected virtual async Task OnMessage( Message message ) {
            await Console.Out.WriteLineAsync( $"{message.From}: {message.MessageText}" );
        }
    }
}
