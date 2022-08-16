using Newtonsoft.Json;
using UnityEngine;

namespace SRC
{
    public class CoreLogger
    {
        public static void Warn(object message) => Debug.Log($"[Warn] {message}");
        public static void Log(object message) => Debug.Log($"[Log] {message}");
        public static void Error(object message) => Debug.Log($"[Error] {message}");

        public static void LogJSON(object message)
        {
            Log(JsonConvert.SerializeObject(message));
        }

        public static void WarnWhenNull(object value, object message)
        {
            if (value == null)
            {
                Warn(message);
            }
        }

        public static void LogWhenTrue(bool value, object message)
        {
            if (value)
            {
                Log(message);
            }
        }

        public static void WarnWhenTrue(bool value, object message)
        {
            if (value)
            {
                Warn(message);
            }
        }
    }
}
