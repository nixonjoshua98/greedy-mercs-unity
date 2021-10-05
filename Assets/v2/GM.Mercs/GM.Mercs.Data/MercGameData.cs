using SimpleJSON;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Mercs.Data
{
    public enum AttackType
    {
        MELEE = 0,
        RANGED = 1
    }

    public class MercGameData
    {
        public MercID Id;
        public AttackType Attack;

        public string Name;

        public double UnlockCost;
        public double BaseDamage;

        public MercPassiveSkillData[] Passives;

        public Sprite Icon;
        public GameObject Prefab;

        public static MercPassiveSkillData[] ParsePassives(JSONArray arr)
        {
            List<MercPassiveSkillData> passives = new List<MercPassiveSkillData>();

            foreach (JSONNode ele in arr)
            {
                MercPassiveSkillData data = new MercPassiveSkillData()
                {
                    Type = (BonusType)ele["bonusType"].AsInt,
                    Value = ele["bonusValue"].AsFloat,
                    UnlockLevel = ele["unlockLevel"].AsInt,
                };

                passives.Add(data);
            }

            return passives.ToArray();
        }
    }
}
