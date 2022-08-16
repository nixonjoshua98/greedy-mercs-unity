using System;
using System.Collections.Generic;
using UnityEngine;

namespace SRC.Common
{
    struct CachedValue
    {
        public DateTime ExpireAt;
        public object Value;
    }

    public class TTLCache
    {
        private readonly Dictionary<string, CachedValue> cacheDict = new();

        public T Get<T>(string key, int lifetime, Func<object> fallback)
        {
            try
            {
                if (!ContainsKey(key))
                {
                    CacheValue(key, lifetime, fallback());
                }

                return (T)cacheDict[key].Value;
            }
            catch (Exception e)
            {
                Debug.LogError(e);

                throw e;
            }
        }

        public T Get<T>(string key, Func<object> factory) => Get<T>(key, int.MaxValue, factory);

        private void CacheValue(string key, int ttl, object obj)
        {
            cacheDict[key] = new CachedValue { ExpireAt = DateTime.UtcNow + TimeSpan.FromSeconds(ttl), Value = obj };
        }

        private bool ContainsKey(string key)
        {
            if (cacheDict.TryGetValue(key, out var result))
            {
                if (DateTime.UtcNow >= result.ExpireAt)
                {
                    cacheDict.Remove(key);

                    return false;
                }

                return true;
            }

            return false;
        }
    }
}
