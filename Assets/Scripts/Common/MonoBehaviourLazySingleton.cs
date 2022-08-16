using UnityEngine;

namespace SRC.Common
{
    public abstract class LazyMonoBehaviour<T> : Core.GMMonoBehaviour where T : Component
    {
        private static readonly object s_Lock = new();
        private static T s_Instance;

        public static T Instance
        {
            get
            {
                lock (s_Lock)
                {
                    if (s_Instance == null)
                    {
                        // Search for existing instance
                        s_Instance = (T)FindObjectOfType(typeof(T));

                        // Create new instance if one doesn't already exist
                        if (s_Instance == null)
                        {
                            // Need to create a new GameObject to attach the singleton to
                            GameObject singletonObject = new(typeof(T).ToString() + " (Lazy Singleton)");

                            s_Instance = singletonObject.AddComponent<T>();

                            // Make instance persistent.
                            DontDestroyOnLoad(singletonObject);
                        }
                    }

                    return s_Instance;
                }
            }
        }
    }
}