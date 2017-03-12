using System;
using Octgn.Chat;
using Topshelf;

namespace Octgn.Online.ChatService
{
    public class TestClient : ServiceControl
    {
        public string To { get; set; }
        private readonly ChatClient _chat;
        private readonly System.Timers.Timer _timer;

        public TestClient(string username, string password) {
            _chat = new ChatClient(username, password);
            _timer = new System.Timers.Timer( 1000 );
            _timer.Elapsed += _timer_Elapsed;
        }

        private void _timer_Elapsed( object sender, System.Timers.ElapsedEventArgs e ) {
            _chat.Send(To, $"Hey there {To}");
        }

        public bool Start( HostControl hostControl ) {
            _chat.Start().Wait(TimeSpan.FromSeconds(10));
            _timer.Start();
            return true;
        }

        public bool Stop( HostControl hostControl ) {
            _chat.Stop().Wait(TimeSpan.FromSeconds(5));
            _timer.Stop();
            return true;
        }
    }
}
