using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Octgn.Chat
{
    public class JoinRoom
    {
        public string SessionId { get; set; }
        public string RoomName { get; set; }
    }

    public class JoinRoomResponse
    {
        public ResultType Result { get; set; }
        public Room Room { get; set; }

        public enum ResultType : byte
        {
            Unknown = 0,
            Ok = 1,
            ErrorTryAgain = 2,
            Unauthorized = 3
        }
    }
}
