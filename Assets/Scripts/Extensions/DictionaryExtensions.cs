using System.Collections.Generic;

namespace GM
{
    public static class DictionaryExtensions
    {
        public static TValue Get<TKey, TValue>(this Dictionary<TKey, TValue> source, TKey key, TValue defaultValue)
        {
            return source.TryGetValue(key, out TValue value) ? value : defaultValue;
        }

        public static TKey GetKeyFromValue<TKey, TValue>(this Dictionary<TKey, TValue> source, TValue value) where TValue : class
        {
            foreach (var pair in source)
            {
                if (pair.Value == value || pair.Value.Equals(value) || (pair.Value == null && value == null))
                {
                    return pair.Key;
                }
            }
            return default;
        }
    }
}
