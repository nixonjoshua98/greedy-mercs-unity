using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Characters
{
    using GM.Targetting;

    public class UnitController : AbstractUnitController
    {
        [SerializeField] CharacterID assignedCharacterId;

        AbstractCharacterAttack autoAttack;

        IAttackTarget attackTargetter;

        void Start()
        {
            GetInitialComponents();
            SubscribeToEvents();
        }

        void GetInitialComponents()
        {
            autoAttack = GetComponent<AbstractCharacterAttack>();

            attackTargetter = GetComponent<IAttackTarget>();

        }

        void SubscribeToEvents()
        {
            autoAttack.E_OnAttackHit.AddListener(OnAttackImpact);
        }


        protected override void PeriodicUpdate()
        {
            if (!autoAttack.HasAttackTarget)
            {
                GameObject target = attackTargetter.GetTarget();

                if (target != null)
                {
                    autoAttack.SetAttackTarget(target);
                }
            }
        }


        void OnAttackImpact(GameObject target)
        {
            // 'target' may be Null under some cases such as an attack being delayed
            if (target && target.TryGetComponent(out AbstractHealthController hp))
            {
                BigDouble dmg = StatsCache.TotalMercDamage(assignedCharacterId);

                StatsCache.ApplyCritHit(ref dmg);

                hp.TakeDamage(dmg);
            }
        }
    }
}
