using UnityEngine;
using HealthController = GM.Controllers.HealthController;

namespace GM.Targets
{
    public interface ITarget
    {
        GameObject GameObject { get; set; }
        TargetType Type { get; set; }
        HealthController Health { get; set; }
        Vector3 Position { get; }
    }

    public abstract class AbstractTarget : ITarget
    {
        public GameObject GameObject { get; set; }
        public TargetType Type { get; set; } = TargetType.Unset;
        public HealthController Health { get; set; }

        public Vector3 Position => GameObject.transform.position;
    }


    public class Target: AbstractTarget
    {
        public Target(GameObject obj, TargetType type)
        {
            GameObject = obj;
            Type = type;

            Health = obj.GetComponent<HealthController>();
        }
    }
}
