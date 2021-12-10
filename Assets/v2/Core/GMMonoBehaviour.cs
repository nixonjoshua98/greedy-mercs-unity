using UnityEngine;

namespace GM.Core
{
    public abstract class GMMonoBehaviour : MonoBehaviour
    {
        protected static GMApplication App => GMApplication.Instance;

        private GameObject MainCanvas => GameObject.FindGameObjectWithTag("MainCanvas");

        public T Instantiate<T>(GameObject obj, Transform parent) where T : Component => Instantiate(obj, parent).GetComponent<T>();
        public T Instantiate<T>(GameObject obj, Transform parent, Vector3 position) where T : Component => Instantiate(obj, position, Quaternion.identity, parent).GetComponent<T>();

        public T InstantiateUI<T>(GameObject obj) where T : Component => Instantiate(obj, MainCanvas.transform).GetComponent<T>();
    }
}
