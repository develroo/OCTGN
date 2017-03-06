using System;
using Octgn.Chat;
using Topshelf;

namespace Octgn.Online.ChatService
{
    public class ChatService : ServiceControl
    {
        private readonly ChatServer _chat;

        public ChatService() {
            _chat = new ChatServer();
        }

        public bool Start( HostControl hostControl ) {
            _chat.Start();
            return true;
        }

        public bool Stop( HostControl hostControl ) {
            _chat.Stop();
            return true;
        }
    }

    public class TestClient : ServiceControl
    {
        private readonly ChatClient _chat;
        private readonly System.Timers.Timer _timer;

        public TestClient() {
            _chat = new ChatClient();
            _timer = new System.Timers.Timer( 1000 );
            _timer.Elapsed += _timer_Elapsed;
        }

        private void _timer_Elapsed( object sender, System.Timers.ElapsedEventArgs e ) {
            _chat.Send( new Message {
                To = "Bill",
                From = "Jim",
                MessageText = "Hey there!"
            } );
        }

        public bool Start( HostControl hostControl ) {
            _chat.Start();
            _timer.Start();
            return true;
        }

        public bool Stop( HostControl hostControl ) {
            _chat.Stop();
            _timer.Stop();
            return true;
        }
    }
}
