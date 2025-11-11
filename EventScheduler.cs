using System;
using System.Collections.Generic;
using System.Linq;

namespace Redox_Code_Test
{
    public class EventScheduler
    {
        private readonly IEventStore _store;
        private readonly List<Event> _events = new();

        public EventScheduler(IEventStore store) => _store = store;

        public void Load()
        {
            _events.Clear();
            _events.AddRange(_store.Load());
            _events.Sort((a, b) => a.DateTime.CompareTo(b.DateTime));
        }

        public void Save() => _store.Save(_events);

        public void ScheduleEvent(Event e)
        {
            if (_events.Any(x => x.DateTime == e.DateTime))
                throw new InvalidOperationException("An event is already scheduled at that exact time.");
            _events.Add(e);
            _events.Sort((a, b) => a.DateTime.CompareTo(b.DateTime));
        }

        public bool CancelEvent(DateTime at)
        {
            var idx = _events.FindIndex(e => e.DateTime == at);
            if (idx < 0) return false;
            _events.RemoveAt(idx);
            return true;
        }

        public IReadOnlyList<Event> GetUpcomingEvents(DateTime from)
        {
            return _events.Where(e => e.DateTime >= from)
                          .OrderBy(e => e.DateTime)
                          .ToList();
        }
    }
}
