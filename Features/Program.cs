using System;
using System.Collections.Generic;
using System.Linq;

namespace Features
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Funcs made for delegates
            Func<int, int> square = x => x * x;
            Func<int, int, int> add = (x, y) => x + y;
            
            // Action
            Action<int> write = x => Console.WriteLine(x);

            // write(square(add(3, 5)));

            IEnumerable<Employee> developers = new Employee[]
            {
                new Employee { Id = 1, Name = "Scrot" },
                new Employee { Id = 2, Name = "Chris" }
            };

            IEnumerable<Employee> sales = new List<Employee>()
            {
                new Employee {Id = 3, Name = "Alex"}
            };

            //Console.WriteLine(developers.Count());
            //IEnumerator<Employee> enumerator = developers.GetEnumerator(); 

            //while (enumerator.MoveNext())
            //{
            //    Console.WriteLine(enumerator.Current.Name);
            //};

            //foreach (var person in developers.Where(delegate (Employee employee)
            //{
            //    return employee.Name.StartsWith("S");
            //}))
            //{
            //    Console.WriteLine(person.Name);
            //}

            var query1 = developers.Where(e => e.Name.Length == 5).OrderBy(e => e.Name);

            var query2 = from developer in developers
                         where developer.Name.Length > 2
                         orderby developer.Name descending
                         select developer;

            foreach (var person in query2)
            {
                Console.WriteLine(person.Name);
            }
        } 
    }
}
