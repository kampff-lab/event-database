using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace EventDatabase
{
    public static class EventWriter
    {
        public static void Write(TextWriter writer, Event evt)
        {
            writer.WriteLine();
        }

        public static Event ReadEvent(TextReader reader)
        {
            var eventText = reader.ReadLine();
            var parameters = eventText.Split(',');
            var evt = new Event();
            if (parameters.Length > 0) evt.Timestamp = DateTimeOffset.Parse(parameters[0]);
            if (parameters.Length > 1) evt.EventType = (EventType)Enum.Parse(typeof(EventType), parameters[1]);
            if (parameters.Length > 2) evt.Value = parameters[2];
            return evt;
        }
    }
}
