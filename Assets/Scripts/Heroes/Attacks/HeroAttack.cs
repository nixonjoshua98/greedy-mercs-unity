using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class HeroAttack: MonoBehaviour
{
    [SerializeField] CharacterID heroId;

    [Space]

    [SerializeField] protected Animator anim;

    [Space]

    [SerializeField, Range(0, 1.0f)] float AttackDelay = 0.25f;

    protected bool isAttacking { get { return anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"); } }

    float attackTimer;

    float lastAttackTime;

    void Awake()
    {
        lastAttackTime = Time.realtimeSinceStartup;

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
        anim.Play("Attack");
    }

    public abstract void OnAttackAnimationEnd();

    protected void DealDamage()
    {
        float timeSinceAttack = Time.realtimeSinceStartup - lastAttackTime;

        GameManager.TryDealDamageToEnemy(StatsCache.GetHeroDamage(heroId) * (timeSinceAttack * Time.timeScale));

        lastAttackTime = Time.realtimeSinceStartup;
    }
}
