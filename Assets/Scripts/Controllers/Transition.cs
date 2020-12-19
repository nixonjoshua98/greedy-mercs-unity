using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Networking;

public class Transition : MonoBehaviour
{
    [SerializeField] Slider slider;

    public void Run(Action callback, Action finishedCallback, float duration)
    {
        StartCoroutine(IRun(callback, finishedCallback, duration));
    }

    IEnumerator IRun(Action callback, Action finishedCallback, float duration)
    {
        yield return ILerp(0, 1, duration);

        callback.Invoke();

        yield return new WaitForSeconds(duration / 2);

        yield return ILerp(1, 0, duration);

        finishedCallback.Invoke();
    }

    IEnumerator ILerp(float a, float b, float t)
    {
        float progress = 0.0f;

        while (progress < 1.0f)
        {
            progress += Time.fixedDeltaTime / t;

            slider.value = Mathf.Lerp(a, b, progress);

            yield return new WaitForEndOfFrame();
        }

        slider.value = b;
    }    
}
