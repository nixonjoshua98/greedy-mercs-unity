using System;
using System.Collections;
using UnityEngine;

namespace SRC
{
    public static class Enumerators
    {
        public static IEnumerator InvokeAfter(Func<bool> predicate, Action callback)
        {
            yield return new WaitUntil(predicate);

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

        public static void InvokeAfter(MonoBehaviour mono, Func<bool> predicate, Action callback)
        {
            mono.StartCoroutine(InvokeAfter(predicate, callback));
        }

        public static IEnumerator LerpFromTo(float from, float to, float duration, Action<float> action)
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
    }
}
