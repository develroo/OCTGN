using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Octgn.Chat
{
    public partial class Room
    {
        public string Name { get; set; }

        public IReadOnlyCollection<string> Users { get; set; }

        internal ChatClient ChatClient { get; set; }

        public Room() { }

        public Room(Room room) {
            Name = room.Name;
            Users = room.Users.ToArray();
        }

        #region Server Side Room Persistance
        private static readonly ConcurrentDictionary<string, Room> _rooms = new ConcurrentDictionary<string, Room>(StringComparer.InvariantCultureIgnoreCase);

        internal static Room Join(string user, string name) {
            var room = Get(name);
            if (room == null) return null;
            var users = new List<string>(room.Users);
            users.Add(user);
            room.Users = users;
            room.Save();
            return room;
        }

        internal static Room Get(string name, Room defaultIfNull = null) {
            return _rooms.GetOrAdd(name, (a) => defaultIfNull ?? new Room {
                Name = name,
                Users = new List<string>()
            });
        }

        internal void ProcessMessage(GroupMessage message) {
            OnMessage?.Invoke(this, new OnMessageEventArgs(message));
        }

        private void Save() {
            // it's references, so currently we don't need to do anything.
        }
        #endregion Server Side Room Persistance

        #region Client Side Implementation
        public event EventHandler<OnMessageEventArgs> OnMessage;
        public async Task Send(string message) {
            if (ChatClient == null) throw new InvalidOperationException(nameof(ChatClient) + " is null");

            await ChatClient.SendToRoom(Name, message);
        }

        public class OnMessageEventArgs : EventArgs
        {
            public GroupMessage Message { get; set; }

            public OnMessageEventArgs() {

            }

            public OnMessageEventArgs(GroupMessage message) {
                Message = message;
            }
        }
        #endregion Client Side Implementation
    }
}
