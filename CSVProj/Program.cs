using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CSVProj
{
    class Program
    {
        static void Main(string[] args)
        {
            var cars = ProcessFile<Car>("fuel.csv");
            var manufacturers = ProcessFile<Manufacturer>("manufacturers.csv");

            var queryGroupJoin = from manufacturer in manufacturers
                                 join car in cars on manufacturer.Name equals car.Division
                                 into carGroup
                                 select new
                                 {
                                     Manufacturer = manufacturer,
                                     cars = carGroup,
                                 } into result
                                 group result by result.Manufacturer.Headquarters;

            //var query = from car in cars
            //            group car by car.Division.ToUpper() into m
            //            orderby m.Key ascending select m;
            //var query2 = cars.GroupBy(c => c.Division.ToUpper()).OrderBy(m => m.Key);

            foreach (var item in queryGroupJoin)
            {
                Console.WriteLine("--------------------------------");
                Console.WriteLine($"{item.Key}");
                Console.WriteLine("--------------------------------");
                foreach (var car in item.SelectMany(g => g.cars.OrderByDescending(c => c.Comb)))
                {
                    Console.WriteLine($"\t{car.Carline} : {car.Comb}");
                }
            }

            // combining two sources of data
            //var query = from car in cars
            //            join manufacturer in manufacturers on new { car.Division, car.Year }  
            //            equals 
            //            new { Division = manufacturer.Name, manufacturer.Year }
            //            orderby car.Comb descending
            //            select new
            //            {
            //                manufacturer.Headquarters,
            //                car.Carline,
            //                car.Comb,
            //                car.Division, 
            //            };
            //var queryFun = cars.Join(manufacturers, 
            //    c => new { c.Division, c.Year}, 
            //    m => new { Division = m.Name, m.Year }, 
            //    (c, m) => new
            //{
            //    m.Headquarters,
            //    c.Carline,
            //    c.Comb,
            //    c.Division,
            //}).OrderByDescending(c => c.Comb).ThenBy(c => c.Carline);
            
            //foreach (var car in query.Take(20))
            //{
            //    Console.WriteLine("--------------------------------------------------------------------------------------------------------------");
            //    Console.WriteLine($" Manufacturer HQ: {car.Headquarters}");
            //    Console.WriteLine($" Car Info: {car.Division} : {car.Carline}");
            //    Console.WriteLine($" Fuel Efficiency: {car.Comb}");
            //    Console.WriteLine("--------------------------------------------------------------------------------------------------------------");
            //}
        }

        public static IEnumerable<T> ProcessFile<T>(string path) where T : class, new()
        {
            var returnRecords = new List<T>();
            var records = File.ReadAllLines(path).Where(line => line.Length > 1).Select(c => c);
            var header = records.First().ToString();
            foreach (var csvLine in records.Skip(1))
            {
                var entity = csvLine.ToEntity<T>(header);
                returnRecords.Add(entity);
            }
            return returnRecords;
        }
    } 

    public static class MyExtensions
    {
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
