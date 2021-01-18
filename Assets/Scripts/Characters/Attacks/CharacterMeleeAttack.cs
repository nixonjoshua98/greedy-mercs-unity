﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GreedyMercs.StageGM.Characters
{
    public class CharacterMeleeAttack : CharacterAttack
    {
        [Header("Character Slots")]
        [SerializeField] GameObject weaponSlot;

        public override void OnAttackEvent()
        {
            DealDamage();
        }

        protected override void OnChangeWeapon(WeaponSO weapon)
        {
            weaponSlot.GetComponent<SpriteRenderer>().sprite = weapon.icon;
        }
    }
}