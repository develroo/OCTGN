using System;
using Octgn.Chat;
using Topshelf;

namespace Octgn.Online.ChatService
{
    public class TestClient : ServiceControl
    {
        public string To { get; set; }

        private Room _lobby;

        private readonly ChatClient _chat;
        private readonly System.Timers.Timer _timer;

        public TestClient(string username, string password) {
            _chat = new ChatClient(username, password);
            _chat.OnMessage += Chat_OnMessage;
            _timer = new System.Timers.Timer( 1000 );
            _timer.Elapsed += _timer_Elapsed;
        }

        private void _timer_Elapsed( object sender, System.Timers.ElapsedEventArgs e ) {
            _chat.Send(To, $"Hey there {To}").Wait();
            _lobby.Send("This is a great group!").Wait();
        }

        public bool Start( HostControl hostControl ) {
            _chat.Start().Wait(TimeSpan.FromSeconds(10));
            _lobby = _chat.JoinRoom("lobby").Result;
            if (_lobby == null) throw new InvalidOperationException(nameof(_lobby) + " is null");
            _lobby.OnMessage += Lobby_OnMessage;
            _timer.Start();
            return true;
        }

        private void Chat_OnMessage(object sender, ChatClient.OnMessageEventArgs e) {
            var message = e.Message;
            Console.Out.WriteLine($"{message.From}: {message.MessageText}");
        }

        private void Lobby_OnMessage(object sender, Room.OnMessageEventArgs e) {
            var message = e.Message;
            Console.Out.WriteLine($"{message.Room}] {message.From}: {message.MessageText}");
        }

        public bool Stop( HostControl hostControl ) {
            _chat.Stop().Wait(TimeSpan.FromSeconds(5));
            _timer.Stop();
            return true;
        }
    }
}
