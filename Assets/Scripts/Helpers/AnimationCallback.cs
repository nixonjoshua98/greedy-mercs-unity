
using UnityEngine;
using UnityEngine.Events;

namespace GreedyMercs
{
    public class AnimationCallback : MonoBehaviour
    {
        public UnityEvent OnAnimationEventCallback;

        public UnityEvent E_Attack;

        public void InvokeAttackEvent()
        {
            E_Attack.Invoke();
        }

        public void OnAnimationEvent()
        {
            OnAnimationEventCallback.Invoke();
        }
    }
}