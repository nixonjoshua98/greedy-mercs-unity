using System.Collections;
using System.Collections.Generic;

using UnityEngine;


namespace GM.Units
{
    using GM.Movement;
    
    public class MeleeCharacterAttack : AbstractCharacterAttack
    {
        float attackRange = 0.75f;

        UnitMovement move;


        void Start()
        {
            move = GetComponent<UnitMovement>();
        }


        protected override void MoveTowardsValidAttackPosition()
        {
            move.MoveTowards(AttackTarget.transform.position);
        }


        protected override bool WithinValidAttackRange()
        {
            float dist = Vector2.Distance(transform.position, AttackTarget.transform.position);

            return dist <= attackRange;
        }
    }
}