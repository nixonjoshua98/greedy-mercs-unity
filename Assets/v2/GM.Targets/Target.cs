using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using HealthController = GM.Controllers.HealthController;

namespace GM.Targets
{
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

        public override bool Equals(object obj)
        {
            Target other = obj as Target;

            if (other == null)
                return false;

            return GameObject == other.GameObject;
        }

        public override int GetHashCode()
        {
            return GameObject.GetHashCode();
        }
    }
}
