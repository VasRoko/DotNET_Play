using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
