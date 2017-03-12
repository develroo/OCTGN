using System;
using Octgn.Chat;
using Topshelf;
using Octgn.Chat.Server;

namespace Octgn.Online.ChatService
{
    public class ChatService : ServiceControl
    {
        private readonly ChatServer _chat;

        public ChatService(string username, string password) {
            _chat = new ChatServer(username, password);
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
}
