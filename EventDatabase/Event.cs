using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventDatabase
{
    public class Event
    {
        public Event()
        {
            Timestamp = DateTimeOffset.Now;
        }

        public DateTimeOffset Timestamp { get; set; }

        public string EventType { get; set; }

        public string Value { get; set; }

        public override string ToString()
        {
            return string.Format("{0},{1},{2}", Timestamp, EventType, Value);
        }

        public static Event Parse(string s)
        {
            var parameters = s.Split(',');
            var evt = new Event();
            if (parameters.Length > 0) evt.Timestamp = DateTimeOffset.Parse(parameters[0]);
            if (parameters.Length > 1) evt.EventType = parameters[1];
            if (parameters.Length > 2) evt.Value = parameters[2];
            return evt;
        }
    }
}
