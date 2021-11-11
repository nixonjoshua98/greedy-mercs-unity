using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM
{
    public static class List_Extensions
    {
        public static T MinBy<T, TProp>(this IEnumerable<T> source, Func<T, TProp> propSelector)
        {
            return source.OrderBy(propSelector).FirstOrDefault();
        }
    }
}
