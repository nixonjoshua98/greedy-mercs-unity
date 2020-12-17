using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class HeroAttack: MonoBehaviour
{
    [SerializeField] HeroID heroId;

    [Space]

    [SerializeField] protected Animator anim;

    [Space]

    [SerializeField, Range(0.0f, 5.0f)] float AttackDelay;

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
        GameManager.TryDealDamageToEnemy(Formulas.CalcHeroDamage(heroId));
    }
}
