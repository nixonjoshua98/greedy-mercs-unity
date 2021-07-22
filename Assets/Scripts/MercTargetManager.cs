using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


namespace GM.Units
{
    using GM.Data;

    public struct AttackTarget
    {
        public GameObject Target;

        public Vector3 Position;
    }


    public class MercTargetManager : MonoBehaviour
    {
        public static MercTargetManager Instance = null;

        Dictionary<GameObject, List<AttackType>> currentTargets;

        void Awake()
        {
            Instance = this;

            currentTargets = new Dictionary<GameObject, List<AttackType>>();
        }


        public GameObject GetTarget(MercID merc)
        {
            UpdateTargetCache();

            if (GetMercAttackType(merc) == AttackType.MELEE)
            {
                return MeleeAttackTarget(merc);
            }

            return null;
        }

        GameObject MeleeAttackTarget(MercID merc)
        {
            GameObject[] targets = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject t in targets)
            {
                if (!currentTargets.TryGetValue(t, out List<AttackType> attackers))
                {
                    AddAttacker(t, AttackType.MELEE);

                    return t;
                }


                int numMeleeAttackers = attackers.Where(type => (type == AttackType.MELEE)).Count();

                if (numMeleeAttackers < 1)
                {
                    AddAttacker(t, AttackType.MELEE);

                    return t;
                }
            }

            return null;
        }


        void UpdateTargetCache()
        {
            GameObject[] keys = currentTargets.Keys.ToArray();

            foreach (GameObject k in keys)
            {
                if (k == null || k.CompareTag("Dead"))
                    currentTargets.Remove(k);
            }
        }

        void AddAttacker(GameObject target, AttackType type)
        {
            if (!currentTargets.ContainsKey(target))
                currentTargets[target] = new List<AttackType>();

            currentTargets[target].Add(type);
        }

        AttackType GetMercAttackType(MercID merc)
        {
            return GameData.Get().Mercs.Get(merc).Attack;
        }
    }
}