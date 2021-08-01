

using UnityEngine;
using UnityEngine.Events;

namespace GM.Units
{
    public class AvatarAnimationEvents : MonoBehaviour
    {
        public UnityEvent E_OnAttackAnimation;
        public UnityEvent E_OnDeathAnimation;

        public void InvokeAttackEvent() => E_OnAttackAnimation.Invoke();

        public void InvokeDeathEvent() => E_OnDeathAnimation.Invoke();
    }
}