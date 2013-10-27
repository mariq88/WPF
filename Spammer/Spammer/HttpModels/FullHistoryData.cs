using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spammer
{
    public class FullHistoryData
    {
        public string Content { get; set; }
        public string Subject { get; set; }
        public string Recipients { get; set; }
        public DateTime SendDate { get; set; }
    }
}
