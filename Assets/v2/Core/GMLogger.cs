using UnityEngine;

namespace GM
{
    public static class GMLogger
    {
        public static void Editor(object obj)
        {
#if UNITY_EDITOR
            Debug.Log(obj);
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

        public static void Log(object obj)
        {
            Debug.Log(obj);
        }
    }
}
