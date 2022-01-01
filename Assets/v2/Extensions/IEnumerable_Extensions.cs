using System;
using System.Collections.Generic;
using System.Linq;

namespace GM
{
    public static class IEnumerable_Extensions
    {
        public static BigDouble Sum(this IEnumerable<BigDouble> source)
        {
            BigDouble total = 0;

            foreach (BigDouble val in source)
            {
                total += val;
            }

            return total;
        }

        public static int FindIndexWhere<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            int index = 0;

            foreach (T ele in source)
            {
                if (predicate(ele))
                {
                    return index;
                }
                index++;
            }

            return -1;
        }
    }
}
