using UnityEngine;
using HealthController = GM.Controllers.HealthController;
using GM.Units;
using GM.Mercs.Controllers;

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

    public abstract class GenericUnitTarget<T> : Target where T : IUnitController
    {
        public T Controller { get; private set; }

        public GenericUnitTarget(GameObject obj, TargetType type)
        {
            GameObject = obj;
            Type = type;

            Health = obj.GetComponent<HealthController>();
            Avatar = obj.GetComponentInChildren<UnitAvatar>();

            Controller = obj.GetComponent<T>();
        }
    }


    public class UnitTarget: GenericUnitTarget<IUnitController>
    {
        public UnitTarget(GameObject obj, TargetType type) : base(obj, type)
        {

        }
    }

    public class MercUnitTarget: GenericUnitTarget<IMercController>
    {
        public MercUnitTarget(GameObject obj) : base(obj, TargetType.Unset)
        {

        }
    }
}
