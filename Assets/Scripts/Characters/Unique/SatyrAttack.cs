using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SatyrAttack : CharacterProjectileAttack
{
    [Header("Character Slots")]
    [SerializeField] protected GameObject weaponSlot;

    public override void CreateProjectile()
    {
        GameObject projectile = Instantiate(ProjectileObject, startPosition.position, Quaternion.identity);

        projectile.GetComponent<Projectile>().Init(OnProjectileHit, 4.0f);
    }

    protected override void OnChangeWeapon(ScriptableWeapon weapon)
    {
        weaponSlot.GetComponent<SpriteRenderer>().sprite = weapon.icon;

        if (weapon.projectile != null)
            ProjectileObject = weapon.projectile;
    }
}
