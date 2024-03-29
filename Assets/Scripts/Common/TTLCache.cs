using System;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Common
{
    internal struct CachedValue
    {
        public DateTime ExpireAt;
        public object Value;
    }

    public class TTLCache
    {
        private readonly Dictionary<string, CachedValue> cacheDict = new Dictionary<string, CachedValue>();

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
                Debug.LogError($"{key} failed to cast to type {typeof(T)}");

                throw e;
            }
        }

        private void CacheValue(string key, int timer, object obj)
        {
            cacheDict[key] = new CachedValue { ExpireAt = DateTime.UtcNow + new TimeSpan(0, 0, timer), Value = obj };
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
