using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CSVProj
{
    class Program
    {
        static void Main(string[] args)
        {
            var cars = ProcessFile("fuel.csv");
            var query = cars.OrderByDescending(c => c.Combined).ThenBy(c => c.Name);
            
            foreach (var car in query.Take(10))
            {
                Console.WriteLine($"{car.Manufacturer}:{car.Name} : {car.Combined}");
                Console.WriteLine("----------------------------");
            }
        }

        private static IEnumerable<Car> ProcessFile(string path)
        {
            return File.ReadAllLines(path).Skip(1).Where(line => line.Length > 1).Select(c => c).ToCar();
        }
    } 

    public static class MyExtensions
    {
        public static IEnumerable<Car> ToCar(this IEnumerable<string> source)
        {
            foreach (var line in source)
            {
                var columns = line.Split(',');

                yield return new Car
                {
                    Year = int.Parse(columns[0]),
                    Manufacturer = columns[1],
                    Name = columns[2],
                    Displacement = double.Parse(columns[3]),
                    Cylinders = int.Parse(columns[4]),
                    City = int.Parse(columns[5]),
                    Highway = int.Parse(columns[6]),
                    Combined = int.Parse(columns[7])
                };
            }
        }
    }
}
