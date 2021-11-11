using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AttackType = GM.Common.Enums.AttackType;

namespace GM.Targets
{
    public class Target
    {
        public GameObject Object;

        public List<AttackType> Attackers { get; private set; } = new List<AttackType>();

        public int AttackersCount => Attackers.Count;

        private Target() { }

        public Target(GameObject obj)
        {
            Object = obj;
        }

        public int AddAttacker(AttackType attacker)
        {
            Attackers.Add(attacker);

            return GetAttackerId();
        }

        int GetAttackerId()
        {
            return AttackersCount - 1;
        }

    }
}
