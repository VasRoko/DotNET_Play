using System;
using LinqClassesLibrary;
using System.Collections.Generic;
using System.Linq;

namespace Queries
{
    public class Program
    {
        static void Main(string[] args)
        {
            var numbers = CustomLinqMethods.Random().Where(n => n > 0.5).Take(20);

            foreach (var number in numbers)
            {
                Console.WriteLine(number);
            }

            var MovieList = new List<Movie>()
            {
                new Movie { Title = "Movie 1", Rating = 8.5f, Year = 2010},
                new Movie { Title = "Movie 2", Rating = 2.1f, Year = 2002},
                new Movie { Title = "Movie 3", Rating = 5.6f, Year = 2005},
                new Movie { Title = "Movie 4", Rating = 8.3f, Year = 2013}
            };

            var query = from movie in MovieList where movie.Year > 2005 orderby movie.Rating descending select movie;

            var enumerator = query.GetEnumerator();
            while(enumerator.MoveNext())
            {
                Console.WriteLine(enumerator.Current.Title); 
            }
        }
    }
}
