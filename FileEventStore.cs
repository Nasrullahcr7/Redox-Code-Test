using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Redox_Code_Test
{
    public sealed class FileEventStore : IEventStore
    {
        private readonly string _path;
        private static readonly JsonSerializerOptions Options = new() { WriteIndented = true };

        public FileEventStore(string path) => _path = path;

        public IReadOnlyList<Event> Load()
        {
            if (!File.Exists(_path)) return new List<Event>();
            var json = File.ReadAllText(_path);
            var data = JsonSerializer.Deserialize<List<Event>>(json, Options);
            return data ?? new List<Event>();
        }

        public void Save(IReadOnlyList<Event> events)
        {
            var json = JsonSerializer.Serialize(events, Options);
            File.WriteAllText(_path, json);
        }
    }
}
