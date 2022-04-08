using System;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Core
{
    public abstract class GMMonoBehaviour : MonoBehaviour
    {
        private readonly Dictionary<Type, object> _CachedComponents = new();

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

        public T Instantiate<T>(GameObject obj, Transform parent)
        {
            return Instantiate(obj, parent).GetComponent<T>();
        }

        public T InstantiateUI<T>(GameObject obj) where T : Component
        {
            return Instantiate(obj, MainCanvas.transform).GetComponent<T>();
        }
    }
}
