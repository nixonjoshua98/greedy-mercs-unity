
using UnityEngine;
using UnityEngine.Events;

namespace GM
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