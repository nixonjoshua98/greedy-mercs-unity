using GM.Mercs.Controllers;
using UnityEngine;
using UnitID = GM.Common.Enums.UnitID;

namespace GM.Mercs
{
    public class SquadMerc
    {
        public MercController Controller { get; private set; }
        public GameObject GameObject { get; private set; }

        // Properties
        public UnitID Id => Controller.Id;
        public Vector3 Position { get => GameObject.transform.position; set => GameObject.transform.position = value; }

        public SquadMerc(GameObject obj)
        {
            GameObject = obj;
            Controller = obj.GetComponent<MercController>();
        }
    }
}
