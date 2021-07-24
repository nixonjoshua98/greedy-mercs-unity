using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GM.Units
{
    public class UnitRangedAttack : UnitAttack
    {
        float attackRange = 2.5f;

        public override Vector3 GetTargetPosition(GameObject target)
        {
            return target.transform.position + (Vector3.left * attackRange);
        }


        public override bool InAttackPosition(GameObject target)
        {
            return Vector3.Distance(transform.position, GetTargetPosition(target)) == 0.0f;
        }
    }
}