using UnityEngine;
using System;
using System.Collections.Generic;

namespace GM.Core
{
    public abstract class GMMonoBehaviour : MonoBehaviour
    {
        Dictionary<Type, object> _CachedComponents = new Dictionary<Type, object>();

        protected GMApplication App => GMApplication.Instance;

        public T GetCachedComponent<T>()
        {
            Type targetType = typeof(T);

            if (!_CachedComponents.TryGetValue(targetType, out object value))
                value = _CachedComponents[targetType] = GetComponent<T>();

            if (value == null)
                return default;

            return (T)value;
        }





        private GameObject MainCanvas => GameObject.FindGameObjectWithTag("MainCanvas");
        public GameObject Instantiate(GameObject obj, Vector3 position) => Instantiate(obj, position, Quaternion.identity, null);
        public T Instantiate<T>(GameObject obj, Transform parent) => Instantiate(obj, parent).GetComponent<T>();
        public T InstantiateUI<T>(GameObject obj) where T : Component => Instantiate(obj, MainCanvas.transform).GetComponent<T>();
    }
}
