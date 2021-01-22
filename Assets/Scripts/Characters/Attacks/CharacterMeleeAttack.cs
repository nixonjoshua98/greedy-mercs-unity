using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GreedyMercs
{
    public class CharacterMeleeAttack : CharacterAttack
    {
        [Header("Character Slots")]
        [SerializeField] GameObject weaponSlot;

        public override void OnAttackEvent()
        {
            DealDamage();
        }
    }
}