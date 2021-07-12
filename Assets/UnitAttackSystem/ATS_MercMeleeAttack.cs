using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Units
{
    public class ATS_MercMeleeAttack : ATS_Attack
    {
        public override Vector3 GetMoveVector(GameObject target)
        {
            return target.transform.position;
        }


        public override bool InAttackPosition(GameObject target)
        {
            float dist = Vector3.Distance(transform.position, target.transform.position);

            return dist <= attackRange;
        }

    }
}
