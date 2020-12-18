using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class AnimationCallback : MonoBehaviour
{
    [SerializeField] public UnityEvent OnAttackAnimationEndedCallback;

    public void OnAttackAnimationEnded()
    {
        TryInvoke(OnAttackAnimationEndedCallback);
    }

    void TryInvoke(UnityEvent e)
    {
        if (e.GetPersistentEventCount() > 0)
            e.Invoke();

        else
            Debug.LogWarning("Animation callback event was triggered but had no listener");
    }
}
