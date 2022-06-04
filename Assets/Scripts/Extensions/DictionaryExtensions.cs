using System.Collections.Generic;

namespace GM
{
    public static class DictionaryExtensions
    {
        public static TValue Get<TKey, TValue>(this Dictionary<TKey, TValue> source, TKey key, TValue defaultValue)
        {
            return source.TryGetValue(key, out TValue value) ? value : defaultValue;
        }

        public static TKey GetKeyFromValue<TKey, TValue>(this Dictionary<TKey, TValue> source, TValue value)
        {
            foreach (var pair in source)
            {
                if (pair.Value.Equals(value))
                {
                    return pair.Key;
                }
            }
            return default;
        }
    }
}
