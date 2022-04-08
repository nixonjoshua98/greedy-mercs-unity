using UnityEngine;
using UnityEngine.SceneManagement;

namespace GM
{
    public static class MonoBehaviourExtensions
    {
        public static T GetComponentInScene<T>(this MonoBehaviour source)
        {
            GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();

            foreach (var rootGameObject in rootGameObjects)
            {
                T component = rootGameObject.GetComponentInChildren<T>();

                if (component is not null)
                {
                    return component;
                }
            }

            return default(T);
        }
    }
}