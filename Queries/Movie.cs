using System;
using System.Collections.Generic;
using System.Text;

namespace Queries
{
    public class Movie
    {
        public string Title { get; set; }
        public float Rating { get; set; }
        
        int _year;
        public int Year 
        { 
            get 
            {
                Console.WriteLine($"returning {_year} for the movie { Title }");
                return _year; 
            } 
            set { _year = value; } 
        }
    }
}
