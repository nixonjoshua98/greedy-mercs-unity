using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM.Extensions
{
    public static class List_Extensions
    {
        public static void UpdateOrInsertElement<T>(this List<T> ls, T val, Predicate<T> predicate)
        {
            for (int i = 0; i < ls.Count; ++i)
            {
                if (predicate(ls[i]))
                {
                    ls[i] = val;

                    return;
                }
            }

            ls.Add(val);
        }
    }
}
