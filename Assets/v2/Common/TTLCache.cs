using System;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Common
{
    struct CachedValue
    {
        public DateTime ExpireAt;
        public object Value;
    }

    public class TTLCache
    {
        Dictionary<string, CachedValue> cacheDict = new Dictionary<string, CachedValue>();

        public void Remove(string key)
        {
            if (cacheDict.ContainsKey(key))
            {
                cacheDict.Remove(key);
            }
        }

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
                Debug.LogError($"{key} failed to case to type {typeof(T)}");

                throw e;
            }
        }

        void CacheValue(string key, int timer, object obj)
        {
            cacheDict[key] = new CachedValue { ExpireAt = DateTime.UtcNow + new TimeSpan(0, 0, timer), Value = obj };
        }

        bool ContainsKey(string key)
        {
            if (cacheDict.TryGetValue(key, out var result))
            {
                if (DateTime.UtcNow >= result.ExpireAt)
                {
                    Remove(key);

                    return false;
                }

                return true;
            }

            return false;
        }
    }
}
