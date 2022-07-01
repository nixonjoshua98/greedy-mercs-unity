﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GM
{
    public static class IEnumerableExtensions
    {
        public static void RemoveAll<TKey, TValue>(this SortedList<TKey, TValue> ls, Predicate<TKey> predicate)
        {
            var keys = ls.Keys.ToList(); // Copy

            foreach (var key in keys)
            {
                if (predicate(key))
                {
                    ls.Remove(key);
                }
            }
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

        public static Vector3 Average(this IEnumerable<Vector3> source)
        {
            Vector3 result = Vector3.zero;

            foreach (Vector3 vec in source)
            {
                result += vec;
            }

            return result / source.Count();
        }
    }
}
