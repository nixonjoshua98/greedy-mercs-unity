using System.Collections;
using System.Collections.Generic;

using UnityEngine;


namespace GM.Characters
{
    public class MeleeCharacterAttack : AbstractCharacterAttack
    {
        public override void Attack(GameObject obj)
        {
            base.Attack(obj);

            anim.Play(AttackAnimation);
        }
    }
}