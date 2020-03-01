using System;
using System.Collections.Generic;
using System.Text;

namespace LinqClassesLibrary
{
    public static class CustomLinqMethods
    {
        public static IEnumerable<T> WhereFilter<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            var result = new List<T>();

            return result;
        }
    }
}
