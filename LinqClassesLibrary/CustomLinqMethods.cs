using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LinqClassesLibrary
{
    public static class CustomLinqMethods
    {
        public static IEnumerable<T> WhereFilter<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            foreach(var item in source)
            {
                if (predicate(item))
                {
                    yield return item;
                }
            }
        }
        public static IEnumerable<double> Random()
        {
            var random = new Random();
            while (true)
            {
                yield return random.NextDouble();
            }
        }
        private static int FindEntityIndex(string entityName, string[] header)
        {
            if (string.IsNullOrEmpty(entityName) || !header.Any())
                throw new ArgumentNullException($"{entityName} or {header} - can not be null");

            int columnIndex = -1;
            foreach (var item in header)
            {
                var columnName = item.Trim().Replace(" ", "").ToLower();
                var normalizedEntityName = entityName.Trim().Replace(" ", "").ToLower();
                if (columnName == normalizedEntityName || columnName.Contains(normalizedEntityName) || normalizedEntityName.StartsWith(columnName))
                {
                    columnIndex = Array.IndexOf(header, item);
                    break;
                }
            }

            return columnIndex;

        }
        private static T CreateType<T>(PropertyInfo[] props, string source, string header) where T : class, new()
        {
            T newType = new T();
            var csvLine = source.Split(",");

            foreach (var prop in props)
            {
                var index = FindEntityIndex(prop.Name, header.Split(","));
                if (index > -1)
                {
                    var lineValue = csvLine[index];
                    var propItem = newType.GetType().GetProperty(prop.Name);

                    switch (prop.PropertyType.Name)
                    {
                        case "DateTime":
                            propItem.SetValue(newType, DateTime.Now);
                            break;
                        case "GUID":
                            propItem.SetValue(newType, Guid.Empty);
                            break;
                        case "String":
                            propItem.SetValue(newType, lineValue);
                            break;
                        case "Int32":
                            propItem.SetValue(newType, Convert.ToInt32(lineValue));
                            break;
                        case "Double":
                            propItem.SetValue(newType, Convert.ToDouble(lineValue));
                            break;
                        default:
                            propItem.SetValue(newType, null);
                            break;
                    }
                };
            }

            return newType;
        }
        public static T ToEntity<T>(this string csvLine, string headerLine) where T : class, new()
        {
            var typeInstance = Activator.CreateInstance(typeof(T));
            var typeProps = typeInstance.GetType().GetProperties();
            return CreateType<T>(typeProps, csvLine, headerLine);
        }
        public static IEnumerable<T> RemoveDuplicates<T>(this IEnumerable<T> source)
        {
            HashSet<T> returnSource = new HashSet<T>(new ObjectComparer<T>());
            foreach (var item in source)
            {
                if (source.Contains(item))
                {
                    returnSource.Add(item);
                }
            }
            return returnSource;
        }
        // ----------------------------------------------------------------------------------- //
        public class ObjectComparer<T> : IEqualityComparer<T>
        {
            public bool Equals(T x, T y)
            {
                var xName = x.GetType().GetProperty("Division").GetValue(x);
                var yName = y.GetType().GetProperty("Division").GetValue(y);

                return String.Equals(xName, yName);
            }

            public int GetHashCode(T obj)
            {
                return obj.GetType().GetProperty("Division").GetValue(obj).GetHashCode();
            }
        }
    }
}
