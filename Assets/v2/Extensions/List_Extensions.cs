using System;
using System.Collections.Generic;

namespace GM
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
