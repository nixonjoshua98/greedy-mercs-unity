
using UnityEngine;
using UnityEngine.Events;

public class AnimationCallback : MonoBehaviour
{
    [SerializeField] public UnityEvent OnAttackAnimationEndedCallback;

    public void OnAttackAnimationEnded()
    {
        OnAttackAnimationEndedCallback.Invoke();
    }
}
