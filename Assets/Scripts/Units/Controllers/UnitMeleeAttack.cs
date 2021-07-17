using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GM.Units
{
    public class UnitMeleeAttack : UnitAttack
    {
        float attackRange = 1.0f;

        public override Vector3 GetMoveVector(GameObject target)
        {
            return GetTargetMovePosition(target);
        }


        public override bool InAttackPosition(GameObject target)
        {
            Vector3 targetMovePosition = GetTargetMovePosition(target);

            float dist = Vector3.Distance(transform.position, targetMovePosition);

            return dist <= attackRange;
        }


        Vector3 GetTargetMovePosition(GameObject target)
        {
            return target.transform.position;
        }
    }
}