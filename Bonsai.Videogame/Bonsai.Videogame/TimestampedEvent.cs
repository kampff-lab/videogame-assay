using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonsai.Videogame
{
    public class TimestampedEvent
    {
        public TimestampedEvent(DateTimeOffset timestamp, string name, string value)
        {
            Timestamp = timestamp;
            Name = name;
            Value = value;
        }

        public DateTimeOffset Timestamp { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }
    }
}
