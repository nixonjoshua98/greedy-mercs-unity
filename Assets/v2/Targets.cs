using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace GM
{
    public enum TargetType
    {
        Unset,
        WaveEnemy,
        Boss
    }

    public class Target
    {
        public GameObject GameObject { get; private set; }
        public TargetType Type { get; private set; } = TargetType.Unset;

        HealthController _Health;
        public HealthController Health
        {
            get
            {
                if (_Health == null && GameObject.TryGetComponent(out HealthController health))
                {
                    _Health = health;
                }
                return _Health;
            }
        }

        public Target(GameObject obj, TargetType type)
        {
            this.GameObject = obj;
            Type = type;
        }
    }


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

        public void Remove(GameObject obj)
        {
            RemoveAll(t => t.GameObject == obj);
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
