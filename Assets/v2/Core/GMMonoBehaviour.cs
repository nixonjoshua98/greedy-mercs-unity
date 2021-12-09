using UnityEngine;

namespace GM.Core
{
    public abstract class GMMonoBehaviour : MonoBehaviour
    {
        protected static GMApplication App => GMApplication.Instance;

        private GameObject SceneMainCanvas => GameObject.FindGameObjectWithTag("MainCanvas");

        public T Instantiate<T>(GameObject obj, Transform parent) where T : Component
        {
            GameObject objInst = Instantiate(obj, parent);

            return objInst.GetComponent<T>();
        }

        public T Instantiate<T>(GameObject obj, Vector3 position) where T : Component
        {
            GameObject objInst = Instantiate(obj, position, Quaternion.identity, SceneMainCanvas.transform);

            return objInst.GetComponent<T>();
        }

        public T InstantiateUI<T>(GameObject obj) where T : Component
        {
            GameObject objInst = Instantiate(obj, SceneMainCanvas.transform);

            return objInst.GetComponent<T>();
        }
    }
}
