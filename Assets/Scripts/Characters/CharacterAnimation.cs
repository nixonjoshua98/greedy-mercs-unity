

using UnityEngine;
using UnityEngine.Events;

namespace GM.Units
{
    public class CharacterAnimation : MonoBehaviour
    {
        public UnityEvent E_OnAttackAnimation;

        public void InvokeAttackEvent()
        {
            E_OnAttackAnimation.Invoke();

            AbstractCharacterAttack controller = GetComponentInParent<AbstractCharacterAttack>();

            if (controller)
            {
                controller.OnAttackAnimationEvent();
            }
        }
    }
}