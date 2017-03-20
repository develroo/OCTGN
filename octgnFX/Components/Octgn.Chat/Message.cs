using System;

namespace Octgn.Chat
{
    public class Message
    {
        public string SessionId { get; set; }
        public string To { get; set; }
        public string From { get; set; }
        public string MessageText { get; set; }
        public DateTimeOffset DateSent { get; set; }
    }

    public class GroupMessage
    {
        public string SessionId { get; set; }
        public string Room { get; set; }
        public string From { get; set; }
        public string MessageText { get; set; }
        public DateTimeOffset DateSent { get; set; }
    }
}
