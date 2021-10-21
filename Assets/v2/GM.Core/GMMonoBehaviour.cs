using UnityEngine;

namespace GM.Core
{
    public abstract class GMMonoBehaviour : MonoBehaviour
    {
        protected static GMApplication App => GMApplication.Instance;

        public T Instantiate<T>(GameObject obj, Transform parent) where T : Component
        {
            GameObject objInst = Instantiate(obj, parent);

            return objInst.GetComponent<T>();
        }

        public T InstantiateUI<T>(GameObject obj) where T : Component
        {
            GameObject mainCanvas = GameObject.FindGameObjectWithTag("MainCanvas");

            GameObject objInst = Instantiate(obj, mainCanvas.transform);

            return objInst.GetComponent<T>();
        }
    }
}
