using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SRC
{
    public static class MonoBehaviourExtensions
    {
        public static void InvokeAfter(this MonoBehaviour mono, float delay, Func<IEnumerator> action)
        {
            IEnumerator _InvokeAfter()
            {
                yield return new WaitForSeconds(delay);
                mono.StartCoroutine(action());
            }

            mono.StartCoroutine(_InvokeAfter());
        }

        public static void InvokeAfter(this MonoBehaviour mono, float delay, Action callback)
        {
            mono.StartCoroutine(InvokeAfter(delay, callback));
        }

        public static void Lerp01(this MonoBehaviour mono, float duration, Action<float> action)
        {
            mono.StartCoroutine(Lerp(mono, 0, 1, duration, action));
        }

        public static T GetComponentInScene<T>(this MonoBehaviour _)
        {
            GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();

            foreach (var rootGameObject in rootGameObjects)
            {
                T component = rootGameObject.GetComponentInChildren<T>();

                if (component is not null)
                    return component;
            }

            return default;
        }

        public static T Instantiate<T>(this MonoBehaviour mono, GameObject obj, Transform parent) => UnityEngine.Object.Instantiate(obj, parent).GetComponent<T>();
        public static GameObject InstantiateUI(this MonoBehaviour mono, GameObject obj) => UnityEngine.Object.Instantiate(obj, MainCanvas.transform);
        public static T InstantiateUI<T>(this MonoBehaviour mono, GameObject obj) => InstantiateUI(mono, obj).GetComponent<T>();

        // Private Static Methods //

        private static GameObject MainCanvas => GameObject.FindGameObjectWithTag("MainCanvas");

        public static IEnumerator Lerp(this MonoBehaviour mono, float from, float to, float duration, Action<float> action)
        {
            float progress = 0;

            while (progress < 1)
            {
                progress += (Time.fixedDeltaTime / duration);

                action.Invoke(Mathf.Lerp(from, to, progress));

                yield return new WaitForFixedUpdate();
            }

            action.Invoke(to);
        }

        static IEnumerator InvokeAfter(float delay, Action callback)
        {
            yield return new WaitForSeconds(delay);

            callback.Invoke();
        }
    }
}