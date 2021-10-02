using UnityEngine;

namespace GM.Common
{
    public abstract class MonoBehaviourLazySingleton<T> : Core.GMMonoBehaviour where T : Component
    {
        static object s_Lock = new object();

        static T s_Instance;

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
                            GameObject singletonObject = new GameObject();

                            s_Instance = singletonObject.AddComponent<T>();

                            singletonObject.name = typeof(T).ToString() + " (Singleton)";

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