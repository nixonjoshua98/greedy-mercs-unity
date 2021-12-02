using System.Collections.Generic;
using UnityEngine;

namespace GM.Targets
{
    public class Target
    {
        public GameObject Object { get; private set; }
        public Vector2? AttackPosition { get; private set; } // Optional position to attack from

        public Target(GameObject obj)
        {
            Object = obj;
        }

        public Target(GameObject obj, Vector2 pos)
        {
            Object = obj;
            AttackPosition = pos;
        }
    }
}