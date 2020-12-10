using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class HeroAttack: MonoBehaviour
{
    protected bool isAttacking;

    [SerializeField] protected Animator anim;

    [Space]

    [SerializeField, Range(0.0f, 5.0f)] float AttackDelay;

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

                if (GameManager.Instance.IsEnemyAvailable)
                {
                    StartAttack();
                }
            }
        }
    }

    protected virtual void StartAttack()
    {
        isAttacking = true;

        anim.Play("Attack");
    }

    public abstract void OnAttackAnimationEnd();

    public virtual void DealDamage()
    {
        if (GameManager.Instance.TryDealDamageToEnemy(1.0f))
        {

        }
    }
}
