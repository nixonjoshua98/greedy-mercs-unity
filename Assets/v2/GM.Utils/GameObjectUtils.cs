using UnityEngine;

namespace GM.Utils
{
    public static class GameObjectUtils
    {
        public static T Instantiate<T>(GameObject obj, Transform parent) where T: Component
        {
            GameObject objInst = Object.Instantiate(obj, parent);

            return objInst.GetComponent<T>();
        }
    }
}
