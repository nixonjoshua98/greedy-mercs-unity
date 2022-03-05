using UnityEngine;

namespace GM.Core
{
    public abstract class GMMonoBehaviour : MonoBehaviour
    {
        protected GMApplication App => GMApplication.Instance;





        private GameObject MainCanvas => GameObject.FindGameObjectWithTag("MainCanvas");

        public GameObject Instantiate(GameObject obj, Vector3 position) => Instantiate(obj, position, Quaternion.identity, null);


        public T Instantiate<T>(GameObject obj, Transform parent) => Instantiate(obj, parent).GetComponent<T>();
        public T InstantiateUI<T>(GameObject obj) where T : Component => Instantiate(obj, MainCanvas.transform).GetComponent<T>();
    }
}
