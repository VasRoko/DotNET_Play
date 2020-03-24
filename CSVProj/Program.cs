using System;
using System.Linq;
using LinqClassesLibrary;

namespace CSVProj
{
    class Program
    {
        static void Main(string[] args)
        {
            var cars = Helpers.ProcessFile<Car>("fuel.csv");
            var manufacturers = Helpers.ProcessFile<Manufacturer>("manufacturers.csv");

            var queryGroupJoin = from manufacturer in manufacturers
                                 join car in cars on manufacturer.Name equals car.Division
                                 into carGroup
                                 select new
                                 {
                                     Manufacturer = manufacturer,
                                     cars = carGroup,
                                 } into result
                                 group result by result.Manufacturer.Headquarters;

            var queryAggregation = from car in cars
                                   group car by car.Carline into carGroup
                                   select new
                                   {
                                       Name = carGroup.Key,
                                       Max = carGroup.Max(c => c.Comb),
                                       Min = carGroup.Min(c => c.Comb),
                                       Avg = carGroup.Average(c => c.Comb)
                                   } into result orderby result.Max descending select result;

            var queryAggregationFun = cars.GroupBy(c => c.Carline).Select(g =>
            {
                var result = g.Aggregate(new CarStatisctics(), (acc, c) => acc.Accumulate(c), acc => acc.Compute());
                return new
                {
                    Name = g.Key,
                    Avg = result.Avg,
                    Min = result.Min,
                    Max = result.Max,
                };
            }).OrderByDescending(r => r.Max);

            //var query = from car in cars
            //            group car by car.Division.ToUpper() into m
            //            orderby m.Key ascending select m;
            //var query2 = cars.GroupBy(c => c.Division.ToUpper()).OrderBy(m => m.Key);
             
            foreach (var item in queryAggregationFun)
            {
                Console.WriteLine("--------------------------------");
                Console.WriteLine($"{item.Name}");
                Console.WriteLine($"\tMax: \t{item.Max}");
                Console.WriteLine($"\tMin: \t{item.Min}");
                Console.WriteLine($"\tAvg: \t{item.Avg}");

                Console.WriteLine("--------------------------------");
                /* foreach (var car in item.SelectMany(g => g.cars.OrderByDescending(c => c.Comb)))
                {
                    Console.WriteLine($"\t{car.Carline} : {car.Comb}");
                } */
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
    } 
}
