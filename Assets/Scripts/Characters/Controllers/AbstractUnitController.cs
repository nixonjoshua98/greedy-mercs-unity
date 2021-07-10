using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GM.Units
{
    using GM.Targetting;

    public abstract class AbstractUnitController : ExtendedMonoBehaviour
    {
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

        void FixedUpdate()
        {
            if (!autoAttack.HasAttackTarget)
                GetNewAttackTarget();

            autoAttack.Process();
        }

        void GetNewAttackTarget()
        {
            GameObject target = attackTargetter.GetTarget();

            if (target != null)
            {
                autoAttack.SetAttackTarget(target);
            }
        }

        protected abstract void OnAttackImpact(GameObject target);
    }
}
