using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

        public bool IsTargetExists(Target target)
        {
            return _targets.Exists(x => x == target);
        }

        public void RemoveTarget(GameObject trgt)
        {
            _targets.RemoveAll(x => x.Object == trgt);
        }

        public bool TryGetTarget(out Target trgt)
        {
            trgt = default;

            if (_targets.Count >= 1)
                trgt = _targets[0];

            return _targets.Count >= 1;
        }

        public bool TryGetTarget(ref Target trgt, GameObject attacker)
        {
            foreach (Target target in _targets.OrderBy(x => Random.value))
            {
                if (target.AttackersCount < 2)
                {
                    trgt = target;

                    target.AddAttacker(attacker);

                    return true;
                }
            }

            return false;
        }
    }
}