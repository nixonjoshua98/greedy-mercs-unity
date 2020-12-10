using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class AnimationCallback : MonoBehaviour
{
    [SerializeField] public UnityEvent OnAttackAnimationEndedCallback;

    [SerializeField] public UnityEvent AnimationEvent_01_Callback;

    public void OnAttackAnimationEnded()
    {
        TryInvoke(OnAttackAnimationEndedCallback);
    }

    public void AnimationEvent_01()
    {
        TryInvoke(AnimationEvent_01_Callback);
    }

    void TryInvoke(UnityEvent e)
    {
        if (e != null)
            e.Invoke();
        else
            Debug.LogWarning("Animation callback event was triggered but had no listener");
    }
}
