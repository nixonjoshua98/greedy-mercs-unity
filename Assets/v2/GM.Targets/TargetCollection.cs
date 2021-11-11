using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AttackType = GM.Common.Enums.AttackType;

namespace GM.Targets
{
    public class TargetCollection
    {
        List<Target> _targets = new List<Target>();

        public int Count => _targets.Count;

        public void Populate(List<GameObject> ls)
        {
            _targets = ls.Select(x => new Target(x)).ToList();
        }

        public bool IsTargetExists(GameObject target)
        {
            return _targets.Exists(x => x.Object == target);
        }

        public bool IsTargetExists(AttackerTarget target)
        {
            return IsTargetExists(target.Object);
        }

        public void RemoveTarget(GameObject trgt)
        {
            _targets.RemoveAll(x => x.Object == trgt);
        }

        public int AddAttacker(AttackerTarget target, AttackType attacker)
        {
            return _targets.Where(x => x.Object == target.Object).First().AddAttacker(attacker);
        }

        public bool TryGetTarget(ref AttackerTarget trgt, AttackType attackerAttackType)
        {
            foreach (Target target in _targets)
            {
                if (target.AttackersCount < 2)
                {
                    trgt = new AttackerTarget
                    {
                        Object = target.Object
                    };

                    return true;
                }
            }

            return false;
        }
    }
}
