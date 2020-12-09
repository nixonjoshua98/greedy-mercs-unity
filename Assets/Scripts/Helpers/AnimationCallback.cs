using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

/*
 * This is mainly used when we have an animation system on a child component and we want to forward an event to another object
 */

public class AnimationCallback : MonoBehaviour
{
    [SerializeField] public UnityEvent OnDyingAnimationEndedCallback;
    [SerializeField] public UnityEvent OnAttackAnimationEndedCallback;

    public void OnDyingAnimationEnded()
    {
        OnDyingAnimationEndedCallback.Invoke();
    }

    public void OnAttackAnimationEnded()
    {
        OnAttackAnimationEndedCallback.Invoke();
    }
}
