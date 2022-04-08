using System;
using System.Collections;
using UnityEngine;

namespace GM
{
    public static class Enumerators
    {
        public static IEnumerator InvokeAfter(Func<bool> predicate, Action callback)
        {
            yield return new WaitUntil(predicate);

            callback.Invoke();
        }

        private static IEnumerator _InvokeAfter(float delay, Action callback)
        {
            yield return new WaitForSecondsRealtime(delay);

            callback.Invoke();
        }

        public static IEnumerator InvokeUntil(Func<bool> predicate, Action action)
        {
            while (!predicate())
            {
                yield return new WaitForFixedUpdate();

                action.Invoke();
            }
        }

        private static IEnumerator LerpFromToCoroutine(float from, float to, float duration, Action<float> action)
        {
            float progress = 0;

            while (progress < 1)
            {
                action.Invoke(Mathf.Lerp(from, to, progress));

                progress += (Time.unscaledDeltaTime / duration);

                yield return new WaitForEndOfFrame();
            }

            action.Invoke(to);
        }

        public static void Lerp01(MonoBehaviour mono, float duration, Action<float> action)
        {
            mono.StartCoroutine(LerpFromToCoroutine(0, 1, duration, action));
        }

        public static void InvokeAfter(MonoBehaviour mono, Func<bool> predicate, Action callback)
        {
            mono.StartCoroutine(InvokeAfter(predicate, callback));
        }

        public static void InvokeAfter(MonoBehaviour mono, float delay, Action callback)
        {
            mono.StartCoroutine(_InvokeAfter(delay, callback));
        }
    }
}
