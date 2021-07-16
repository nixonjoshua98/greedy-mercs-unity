

using UnityEngine;
using UnityEngine.Events;

namespace GM.Units
{
    public class CharacterAnimation : MonoBehaviour
    {
        public UnityEvent E_OnAttackAnimation;
        public UnityEvent E_OnDeathAnimation;

        public void InvokeAttackEvent()
        {
            E_OnAttackAnimation.Invoke();

            AbstractCharacterAttack controller = GetComponentInParent<AbstractCharacterAttack>();

            if (controller)
            {
                controller.OnAttackAnimationEvent();
            }
        }

        public void InvokeDeathEvent() => E_OnDeathAnimation.Invoke();
    }
}