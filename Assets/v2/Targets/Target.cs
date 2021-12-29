using GM.Units;
using UnityEngine;
using HealthController = GM.Controllers.HealthController;

namespace GM.Targets
{
    public abstract class Target
    {
        public GameObject GameObject { get; set; }

        public TargetType Type { get; set; } = TargetType.Unset;

        public HealthController Health { get; protected set; }
        public UnitAvatar Avatar { get; protected set; }

        public Vector3 Position => GameObject.transform.position;
    }

    public abstract class GenericTarget<T> : Target where T : IUnitController
    {
        public T Controller { get; private set; }

        public GenericTarget(GameObject obj, TargetType type)
        {
            GameObject = obj;
            Type = type;

            Health = obj.GetComponent<HealthController>();
            Avatar = obj.GetComponentInChildren<UnitAvatar>();

            Controller = obj.GetComponent<T>();
        }
    }


    public class UnitTarget: GenericTarget<IUnitController>
    {
        public UnitTarget(GameObject obj, TargetType type) : base(obj, type)
        {

        }
    }
}
