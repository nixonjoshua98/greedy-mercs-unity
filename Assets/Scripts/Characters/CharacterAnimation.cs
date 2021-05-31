

using UnityEngine;

namespace GM.Characters
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