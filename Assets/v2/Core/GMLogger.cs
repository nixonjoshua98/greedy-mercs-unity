using UnityEngine;
using System;

namespace GM
{
    public static class GMLogger
    {
        public static void Editor(object obj)
        {
#if UNITY_EDITOR
            Debug.Log($"[Editor] {obj}");
#endif
        }

        public static void WhenNull(object obj, object msg, bool editor = false)
        {
            if (ReferenceEquals(obj, null))
            {
                if (editor)
                {
                    GMLogger.Editor(msg);
                }
                else
                {
                    Debug.Log(msg);
                }
            }           
        }

        public static void Exception(string msg, Exception e)
        {
            Debug.LogError($"{msg}\n{e.Message}");
        }

        public static void JSON(object obj)
        {
            Debug.Log(Newtonsoft.Json.JsonConvert.SerializeObject(obj));
        }
    }
}
