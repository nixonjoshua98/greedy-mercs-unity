using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHeroAttack : HeroAttack
{
    [SerializeField] ParticleSystem attackParticle;

    protected override void OnAttackAnimationEnd()
    {
        attackParticle.Play();
    }

    public void OnAttackParticleSystemStopped()
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
