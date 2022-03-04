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

        public static void InvokeAfter(MonoBehaviour mono, Func<bool> predicate, Action callback) => mono.StartCoroutine(InvokeAfter(predicate, callback));
    }
}
