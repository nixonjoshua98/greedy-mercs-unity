using UnityEngine;

namespace GM.Core
{
    public abstract class GMMonoBehaviour : MonoBehaviour
    {
        protected static GMApplication App => GMApplication.Instance;










        private GameObject MainCanvas => GameObject.FindGameObjectWithTag("MainCanvas");

        public GameObject Instantiate(GameObject obj, Vector3 position) => Instantiate(obj, position, Quaternion.identity, null);


        public T Instantiate<T>(GameObject obj, Transform parent) => Instantiate(obj, parent).GetComponent<T>();
        public T Instantiate<T>(GameObject obj, Transform parent, Vector3 position) => Instantiate(obj, position, Quaternion.identity, parent).GetComponent<T>();
        public T Instantiate<T>(GameObject obj, Vector3 position) => Instantiate(obj, position, Quaternion.identity, null).GetComponent<T>();

        public GameObject InstantiateUI(GameObject obj)  => Instantiate(obj, MainCanvas.transform);
        public T InstantiateUI<T>(GameObject obj) where T : Component => Instantiate(obj, MainCanvas.transform).GetComponent<T>();
    }
}
