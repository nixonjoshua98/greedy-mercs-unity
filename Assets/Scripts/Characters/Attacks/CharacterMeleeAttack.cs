using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GreedyMercs
{
    public class CharacterMeleeAttack : CharacterAttack
    {
        public override void OnAttackAnimationFinished()
        {
            OnAttackHit();
        }
    }
}