using System;
using System.Collections.Generic;
using System.Linq;

namespace GM
{
    public static class List_Extensions
    {
        public static T MinBy<T, TProp>(this IEnumerable<T> source, Func<T, TProp> propSelector)
        {
            return source.OrderBy(propSelector).FirstOrDefault();
        }

        public static BigDouble Sum(this IEnumerable<BigDouble> source)
        {
            BigDouble total = 0;

            foreach (BigDouble val in source)
            {
                total += val;
            }

            return total;
        }
    }
}
