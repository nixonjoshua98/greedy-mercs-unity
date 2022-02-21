using GM.Units;
using UnityEngine;

namespace GM.Targets
{
    public class Target
    {
        public GameObject GameObject { get; set; }

        public Target(GameObject obj)
        {
            GameObject = obj;
        }
    }
}
