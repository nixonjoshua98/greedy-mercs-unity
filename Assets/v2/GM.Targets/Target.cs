using System.Collections.Generic;
using UnityEngine;

namespace GM.Targets
{
    public class Target
    {
        public GameObject Object { get; private set; }
        List<GameObject> Attackers = new List<GameObject>();
        public int AttackersCount { get => Attackers.Count; }
        public Vector2? AttackPosition { get; private set; } // Optional position to attack from

        private Target() { }

        public Target(GameObject obj)
        {
            Object = obj;
        }

        public Target(GameObject obj, Vector2 pos)
        {
            Object = obj;
            AttackPosition = pos;
        }

        public void AddAttacker(GameObject attacker)
        {
            Attackers.Add(attacker);
        }
    }
}