using UnityEngine;
using HealthController = GM.Controllers.HealthController;

namespace GM.Targets
{
    public interface ITarget
    {
        GameObject GameObject { get; set; }
        TargetType Type { get; set; }
        HealthController Health { get; }
    }

    public abstract class AbstractTarget : ITarget
    {
        public GameObject GameObject { get; set; }
        public TargetType Type { get; set; } = TargetType.Unset;

        HealthController _Health;
        public HealthController Health
        {
            get
            {
                if (_Health == null && GameObject.TryGetComponent(out HealthController health))
                    _Health = health;

                return _Health;
            }
        }
    }


    public class Target: AbstractTarget
    {
        public Target(GameObject obj, TargetType type)
        {
            GameObject = obj;
            Type = type;
        }
    }
}
