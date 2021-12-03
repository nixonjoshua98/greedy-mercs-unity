using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using HealthController = GM.Controllers.HealthController;

namespace GM.Targets
{
    public class TargetList : List<Target>
    {
        public Target Add(GameObject targetObject, TargetType targetType)
        {
            Target t = new Target(targetObject, targetType);

            Add(t);

            return t;
        }

        public void AddRange(IEnumerable<GameObject> ls, TargetType targetType)
        {
            foreach (GameObject o in ls)
            {
                Add(new Target(o, targetType));
            }
        }

        public bool TryGetWithType(TargetType type, ref Target target)
        {
            List<Target> ls = this.Where(t => t.Type == type).ToList();

            if (ls.Count > 0)
            {
                target = ls[0];
            }

            return target != null;
        }
    }
}
