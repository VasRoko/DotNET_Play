using System;
using System.Collections.Generic;
using System.Linq;

namespace Queries
{
    class Program
    {
        static void Main(string[] args)
        {
            var MovieList = new List<Movie>()
            {
                new Movie { Title = "Movie 1", Rating = 8.5f, Year = 2010},
                new Movie { Title = "Movie 1", Rating = 8.5f, Year = 2002},
                new Movie { Title = "Movie 1", Rating = 8.5f, Year = 2005},
                new Movie { Title = "Movie 1", Rating = 8.5f, Year = 2013}
            };

            var query = MovieList.Where(x => x.Year > 2008);

            foreach(Movie movie in query)
            {
                Console.WriteLine(movie.Year);
            }
        }
    }
}
