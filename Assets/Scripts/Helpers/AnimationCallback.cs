
using UnityEngine;
using UnityEngine.Events;

public class AnimationCallback : MonoBehaviour
{
    [SerializeField] public UnityEvent OnAnimationEventCallback;

    public void OnAnimationEvent()
    {
        OnAnimationEventCallback.Invoke();
    }
}
