using UnityEngine;

namespace GM.Core
{
    public abstract class GMMonoBehaviour : MonoBehaviour
    {
        protected static GMApplication App => GMApplication.Instance;

        public static T Instantiate<T>(GameObject obj, Transform parent) where T : Component
        {
            GameObject objInst = Instantiate(obj, parent);

            return objInst.GetComponent<T>();
        }
    }
}
