using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public abstract class CharacterProjectileAttack : CharacterAttack
{
    [SerializeField] protected GameObject ProjectileObject;

    [SerializeField] protected Transform startPosition;

    public override void OnAttackEvent()
    {
        CreateProjectile();
    }

    public abstract void CreateProjectile();

    public virtual void OnProjectileHit()
    {
        DealDamage();
    }
}
