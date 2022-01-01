using GM.Units;
using UnityEngine;
using HealthController = GM.Controllers.HealthController;

namespace GM.Targets
{
    public interface ITarget
    {
        GameObject GameObject { get; set; }
        TargetType Type { get; set; }
        HealthController Health { get; set; }
        UnitAvatar Avatar { get; set; }
        Vector3 Position { get; }
    }

    public class Target : ITarget
    {
        public IUnitController Controller { get; set; }
        public HealthController Health { get; set; }
        public GameObject GameObject { get; set; }
        public UnitAvatar Avatar { get; set; }

        public TargetType Type { get; set; } = TargetType.Unset;

        public Vector3 Position => GameObject.transform.position;

        public Target(GameObject obj, TargetType type)
        {
            GameObject = obj;
            Type = type;

            Health = obj.GetComponent<HealthController>();
            Avatar = obj.GetComponentInChildren<UnitAvatar>();

            Controller = obj.GetComponent<IUnitController>();
        }
    }
}
