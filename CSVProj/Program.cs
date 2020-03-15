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

            var query = from car in cars
                        join manufacturer in manufacturers on new { car.Division, car.Year }  
                        equals 
                        new { Division = manufacturer.Name, manufacturer.Year }
                        orderby car.Comb descending
                        select new
                        {
                            manufacturer.Headquarters,
                            car.Carline,
                            car.Comb,
                            car.Division, 
                        };
            var queryFun = cars.Join(manufacturers, 
                c => new { c.Division, c.Year}, 
                m => new { Division = m.Name, m.Year }, 
                (c, m) => new
            {
                m.Headquarters,
                c.Carline,
                c.Comb,
                c.Division,
            }).OrderByDescending(c => c.Comb).ThenBy(c => c.Carline);
            
            foreach (var car in queryFun.Take(20))
            {
                Console.WriteLine("--------------------------------------------------------------------------------------------------------------");
                Console.WriteLine($" Manufacturer HQ: {car.Headquarters}");
                Console.WriteLine($" Car Info: {car.Division} : {car.Carline}");
                Console.WriteLine($" Fuel Efficiency: {car.Comb}");
                Console.WriteLine("--------------------------------------------------------------------------------------------------------------");
            }
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
    }
}
