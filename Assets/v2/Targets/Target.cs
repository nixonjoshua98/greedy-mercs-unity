using GM.Units;
using UnityEngine;

namespace GM.Targets
{
    public interface ITarget
    {
        GameObject GameObject { get; set; }
        UnitAvatar Avatar { get; set; }
        Vector3 Position { get; }
    }

    public class Target : ITarget
    {
        public GameObject GameObject { get; set; }
        public UnitAvatar Avatar { get; set; }

        public Vector3 Position => GameObject.transform.position;

        public Target(GameObject obj)
        {
            GameObject = obj;

            Avatar = obj.GetComponentInChildren<UnitAvatar>();
        }
    }
}
