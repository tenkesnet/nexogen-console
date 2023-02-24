using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexogenLogger
{
    public class LongLogMessageException : Exception
    {
        public LongLogMessageException(string message) : base(message)
        {
        }
    }
}
