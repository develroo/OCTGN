using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Octgn.Chat
{
    public class Room
    {
        public string Name { get; set; }

        public IReadOnlyCollection<string> Users { get; set; }
    }
}
