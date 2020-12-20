using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class MeleeHeroAttack : HeroAttack
{
    public override void OnAttackAnimationEnd()
    {
        DealDamage();
    }
}
