using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spammer
{
    public class SendEmailData
    {
        public List<string> Emails { get; set; }

        public string Subject { get; set; }

        public string Content { get; set; }
    }
}
