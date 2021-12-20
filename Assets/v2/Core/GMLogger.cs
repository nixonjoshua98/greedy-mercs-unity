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

        public static void WhenNull(object obj, object msg)
        {
            if (obj == null)
            {
                Debug.Log(msg);
            }
        }
    }
}
