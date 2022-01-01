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

        public static IEnumerator InvokeAfter(IEnumerator coroutine, Action action)
        {
            yield return coroutine;

            action.Invoke();
        }
    }
}
