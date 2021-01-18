using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GreedyMercs
{
    public class SatyrAttack : CharacterProjectileAttack
    {
        [Header("Character Slots")]
        [SerializeField] protected GameObject weaponSlot;

        public override void CreateProjectile()
        {
            GameObject projectile = Instantiate(ProjectileObject, startPosition.position, Quaternion.identity);

            projectile.GetComponent<StraightProjectile>().Init(OnProjectileHit, 4.0f);
        }

        protected override void OnChangeWeapon(WeaponSO weapon)
        {
            weaponSlot.GetComponent<SpriteRenderer>().sprite = weapon.icon;

            ProjectileObject = weapon.prefab;
        }
    }
}