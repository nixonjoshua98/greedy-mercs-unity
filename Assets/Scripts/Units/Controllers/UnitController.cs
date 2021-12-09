using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using GM.Targets;

namespace GM.Units
{
    public abstract class UnitController : Core.GMMonoBehaviour
    {
        protected abstract TargetList<Target> CurrentTargetList { get; }

        protected bool IsTargetValid(Target target)
        {
            if (target == null || target.GameObject == null)
            {
                return false;
            }
            else if (target.Health.IsDead)
            {
                return false;
            }

            return true;
        }

        protected bool TryGetBossFromTargetList(ref Target boss)
        {
            return CurrentTargetList.TryGetWithType(TargetType.Boss, ref boss);
        }

        protected abstract Target GetTargetFromTargetList();
    }
}
