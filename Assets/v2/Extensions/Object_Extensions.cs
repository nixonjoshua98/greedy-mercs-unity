using UnityEngine;
using UnityEngine.SceneManagement;

namespace GM
{
    public static class Object_Extensions
    {
        public static T GetComponentInScene<T>(this MonoBehaviour @this)
        {
            GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();

            foreach (var rootGameObject in rootGameObjects)
            {
                T component = rootGameObject.GetComponentInChildren<T>();

                if (component != null)
                {
                    return component;
                }
            }

            return default(T);
        }
    }
}