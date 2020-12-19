using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class HeroAttack: MonoBehaviour
{
    [SerializeField] HeroID heroId;

    [Space]

    [SerializeField] protected Animator anim;

    [Space]

    float AttackDelay = 0.25f;

    protected bool isAttacking;

    float attackTimer;

    void Awake()
    {
        isAttacking = false;

        attackTimer = AttackDelay;
    }

    void FixedUpdate()
    {
        if (!isAttacking)
        {
            attackTimer -= Time.fixedDeltaTime;

            if (attackTimer <= 0.0f)
            {
                attackTimer = AttackDelay;

                if (GameManager.IsEnemyAvailable)
                    StartAttack();
            }
        }
    }

    void StartAttack()
    {
        isAttacking = true;

        anim.Play("Attack");
    }

    public abstract void OnAttackAnimationEnd();

    protected void DealDamage()
    {
        GameManager.TryDealDamageToEnemy(HeroStatsCache.GetHeroDamage(heroId));
    }
}
