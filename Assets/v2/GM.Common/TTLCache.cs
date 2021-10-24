using System;
using System.Collections.Generic;
using System.Linq;

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

        long cacheHitsCounter = 0;

        public void Remove(string key)
        {
            if (cacheDict.ContainsKey(key))
            {
                cacheDict.Remove(key);
            }
        }

        public T Get<T>(string key, int lifetime, Func<object> fallback)
        {
            if (!ContainsKey(key))
            {
                cacheHitsCounter++;

                if (cacheHitsCounter % 100 == 0)
                {
                    ClearExpiredValues();
                }

                CacheValue(key, lifetime, fallback());
            }

            return (T)cacheDict[key].Value;
        }

        void ClearExpiredValues()
        {
            DateTime now = DateTime.UtcNow;

            foreach (string key in cacheDict.Keys.ToList())
            {
                if (now > cacheDict[key].ExpireAt)
                {
                    Remove(key);
                }
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
                if (DateTime.UtcNow > result.ExpireAt)
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
