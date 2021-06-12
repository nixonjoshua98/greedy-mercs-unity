using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GM.Characters
{
    public class ParticleCharacterAttack : AbstractCharacterAttack
    {
        [Header("Components")]
        [SerializeField] ParticleSystem ps;


        protected override void MoveTowardsValidAttackRange()
        {

        }

        protected override bool WithinValidAttackRange()
        {
            return true;
        }


        public override void Attack(GameObject obj)
        {
            base.Attack(obj);

            anim.Play(AttackAnimation);
        }

        public override void OnAttackAnimationEvent()
        {
            ps.Play();
        }

        public void OnParticleDoneEvent()
        {
            OnAttackHit();
        }
    }
}
