using System.Collections.Generic;

namespace GM
{
    public static class Dictionary_Extensions
    {
        public static TValue Get<TKey, TValue>(this Dictionary<TKey, TValue> source, TKey key, TValue defaultValue)
        {
            return source.TryGetValue(key, out TValue value) ? value : defaultValue;
        }
    }
}
