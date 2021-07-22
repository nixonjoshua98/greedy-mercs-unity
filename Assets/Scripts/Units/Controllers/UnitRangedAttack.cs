using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GM.Units
{
    public class UnitRangedAttack : UnitAttack
    {
        float attackRangeY = 0.25f;
        float attackRangeX = 2.5f;

        public override Vector3 GetTargetPosition(GameObject target)
        {
            bool inXRange = WithinXRange(target);
            bool inYRange = WithinYRange(target);

            if (!inXRange && !inYRange)
                return target.transform.position;

            else if (!inYRange)
                return new Vector3(transform.position.x, target.transform.position.y);

            return new Vector3(target.transform.position.x, transform.position.y);
        }


        public override bool InAttackPosition(GameObject target)
        {
            return WithinYRange(target) && WithinXRange(target);
        }


        bool WithinYRange(GameObject target) => Mathf.Abs(transform.position.y - target.transform.position.y) <= attackRangeY;
        bool WithinXRange(GameObject target) => Mathf.Abs(transform.position.x - target.transform.position.x) <= attackRangeX;
    }
}