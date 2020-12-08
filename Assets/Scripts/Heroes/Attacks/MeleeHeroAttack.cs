using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class MeleeHeroAttack : HeroAttack
{
    protected override void OnAttackAnimationEnd()
    {
        isAttacking = false;

        DealDamage();
    }

    public override void DealDamage()
    {
        if (GameManager.Instance.TryDealDamageToEnemy(1.0f))
        {

        }
    }
}
