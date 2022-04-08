using System;
using UnityEngine;

namespace GM
{
    public static class GMLogger
    {
        public static void Editor(object obj)
        {
            if (Application.isEditor)
                Debug.Log($"[Editor] {obj}");
        }

        public static void WhenNull(object obj, object msg, bool editorOnly = false)
        {
            if (obj == null && ((Application.isEditor && editorOnly) || !editorOnly))
            {
                Debug.Log(msg);
            }
        }

        public static void Exception(string msg, Exception e, bool editorOnly = false)
        {
            if ((Application.isEditor && editorOnly) || !editorOnly)
                Debug.LogError($"{msg}\n{e.Message}");
        }

        public static void JSON(object obj)
        {
            Debug.Log(Newtonsoft.Json.JsonConvert.SerializeObject(obj));
        }
    }
}
