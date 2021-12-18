using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GM.Units
{
    public class UnitMeleeAttack : UnitAttack
    {
        protected override Vector3 GetTargetPosition(GameObject target)
        {
            return target.transform.position;
        }


        protected override bool InAttackPosition(GameObject target)
        {
            return Vector3.Distance(transform.position, GetTargetPosition(target)) == 0.0f;
        }
    }
}