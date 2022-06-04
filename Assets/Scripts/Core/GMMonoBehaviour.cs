using UnityEngine;

namespace GM.Core
{
    public abstract class GMMonoBehaviour : MonoBehaviour
    {
        protected GMApplication App => GMApplication.Instance;


        /* Helper Instantiate Methods (could be extensions instead) */
        private GameObject MainCanvas => GameObject.FindGameObjectWithTag("MainCanvas");
        public T Instantiate<T>(GameObject obj, Transform parent) => Instantiate(obj, parent).GetComponent<T>();
        public GameObject InstantiateUI(GameObject obj) => Instantiate(obj, MainCanvas.transform);
        public T InstantiateUI<T>(GameObject obj) => InstantiateUI(obj).GetComponent<T>();
    }
}
