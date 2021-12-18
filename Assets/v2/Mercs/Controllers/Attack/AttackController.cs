using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GM.Targets;

namespace GM.Mercs.Controllers
{
    public interface IAttackController
    {
        public bool IsAttacking();
        public void StartAttack(Target target, Action callback);
        public bool InAttackPosition(Target target);
        public void MoveTowardsAttackPosition(Target target);
    }

    public abstract class AttackController : GM.Core.GMMonoBehaviour, IAttackController
    {
        public abstract bool IsAttacking();
        public abstract bool InAttackPosition(Target target);
        public abstract void StartAttack(Target target, Action callback);
        public abstract void MoveTowardsAttackPosition(Target target);
    }
}
