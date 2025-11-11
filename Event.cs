using System;

namespace Redox_Code_Test
{
    public class Event
    {
        public string Name { get; }
        public string Location { get; }
        public DateTime DateTime { get; }

        public Event(string name, string location, DateTime dateTime)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required.", nameof(name));
            if (string.IsNullOrWhiteSpace(location)) throw new ArgumentException("Location is required.", nameof(location));
            Name = name;
            Location = location;
            DateTime = dateTime;
        }
    }
}
