using UnityEngine;
using HealthController = GM.Controllers.HealthController;
using GM.Units;


namespace GM.Targets
{
    public abstract class AbstractTarget
    {
        public GameObject GameObject { get; set; }
        public TargetType Type { get; set; } = TargetType.Unset;
        public HealthController Health { get; protected set; }
        public UnitAvatar Avatar { get; protected set; }

        public Vector3 Position => GameObject.transform.position;
    }


    public class Target: AbstractTarget
    {
        public Target(GameObject obj, TargetType type)
        {
            GameObject = obj;
            Type = type;

            Health = obj.GetComponent<HealthController>();
            Avatar = obj.GetComponentInChildren<UnitAvatar>();
        }
    }
}
