using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexogenLogger
{
    public class LogItem
    {
        public DateTime time { get; set; }
        public string message { get; set; }
        public LogLevelEnum level { get; set; }
    }
}
