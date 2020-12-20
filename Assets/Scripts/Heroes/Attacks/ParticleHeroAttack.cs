using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ParticleHeroAttack : HeroAttack
{
    [SerializeField] ParticleSystem attackParticle;

    public override void OnAttackAnimationEnd()
    {
        attackParticle.Play();
    }

    public void OnAttackParticleSystemStopped()
    {
        DealDamage();
    }
}
