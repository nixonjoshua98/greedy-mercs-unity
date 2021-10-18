using System;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Common
{
    struct CachedValue
    {
        public DateTime CachedTime;
        public object Value;
    }

    public class TTLCache
    {
        Dictionary<string, CachedValue> _Cache = new Dictionary<string, CachedValue>();

        float DefaultLifeTime;

        private TTLCache() { }

        public TTLCache(float lifetime)
        {
            DefaultLifeTime = lifetime;
        }

        public void Clear() => _Cache.Clear();

        public T Get<T>(string key, Func<object> fallback) => Get<T>(key, DefaultLifeTime, fallback);
        public T Get<T>(string key, float lifetime, Func<object> fallback)
        {
            if (!TryGetValue(key, lifetime, out CachedValue result))
            {
                CacheValue(key, fallback);
            }

            return (T)_Cache[key].Value;
        }

        void CacheValue(string key, Func<object> func)
        {
            _Cache[key] = new CachedValue { CachedTime = DateTime.UtcNow, Value = func() };
        }


        bool TryGetValue(string key, float lifetime, out CachedValue result)
        {
            if (_Cache.TryGetValue(key, out result))
            {
                TimeSpan timeSinceCached = DateTime.UtcNow - result.CachedTime;

                if (timeSinceCached.TotalSeconds > lifetime)
                {
                    _Cache.Remove(key);

                    return false;
                }

                return true;
            }

            return false;
        }
    }
}
