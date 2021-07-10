

using UnityEngine;

namespace GM.Units
{
    public class CharacterAnimation : MonoBehaviour
    {
        public void InvokeAttackEvent()
        {
            AbstractCharacterAttack controller = GetComponentInParent<AbstractCharacterAttack>();

            controller.OnAttackAnimationEvent();
        }
    }
}