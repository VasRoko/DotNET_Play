using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace LinqClassesLibrary
{
    public static class Helpers
    {
        public static IEnumerable<T> ProcessFile<T>(string path) where T : class, new()
        {
            var returnRecords = new List<T>();
            var records = File.ReadAllLines(path).Where(line => line.Length > 1).Select(c => c);
            var header = records.First().ToString();
            foreach (var line in records.Skip(1))
            {
                var entity = line.ToEntity<T>(header);
                returnRecords.Add(entity);
            }
            return returnRecords;
        }
    }
}
