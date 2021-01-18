
using UnityEngine;
using UnityEngine.Events;

namespace GreedyMercs
{
    public class AnimationCallback : MonoBehaviour
    {
        [SerializeField] public UnityEvent OnAnimationEventCallback;

        public void OnAnimationEvent()
        {
            OnAnimationEventCallback.Invoke();
        }
    }
}