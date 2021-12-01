using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM
{
    public static class Dictionary_Extensions
    {
        public static TValue Get<TKey, TValue>(this Dictionary<TKey, TValue> @this, TKey key, TValue defaultValue)
        {
            if (@this.TryGetValue(key, out TValue value))
            {
                return value;
            }
            return defaultValue;
        }
    }
}
