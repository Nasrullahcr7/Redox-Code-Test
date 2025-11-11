using System.Collections.Generic;

namespace Redox_Code_Test
{
    public interface IEventStore
    {
        IReadOnlyList<Event> Load();
        void Save(IReadOnlyList<Event> events);
    }
}