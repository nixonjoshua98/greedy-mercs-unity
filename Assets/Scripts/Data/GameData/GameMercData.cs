using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using SimpleJSON;

namespace GM.Units
{
    public enum AttackType
    {
        MELEE = 0,
        RANGED = 1
    }
}


namespace GM.Data
{
    using AttackType = Units.AttackType;

    public struct MercPassiveData
    {
        public BonusType Type;

        public float Value;

        public int UnlockLevel;
    }


    public class MercData
    {
        public MercID Id;
        public AttackType Attack;

        public string Name;

        public double UnlockCost;
        public double BaseDamage;

        public MercPassiveData[] Passives;

        public Sprite Icon;

        public GameObject Prefab;

        public MercData(LocalMercData local, JSONNode node)
        {
            Id      = local.ID;
            Name    = local.Name;
            Icon    = local.Icon;
            Prefab  = local.Prefab;

            Attack      = (AttackType)node["attackType"].AsInt;
            BaseDamage  = node["baseDamage"].AsDouble;
            UnlockCost  = node["unlockCost"].AsDouble;

            Passives = ParsePassives(node["passives"].AsArray);
        }


       static MercPassiveData[] ParsePassives(JSONArray arr)
        {
            List<MercPassiveData> passives = new List<MercPassiveData>();

            foreach (JSONNode ele in arr)
            {
                MercPassiveData data = new MercPassiveData()
                {
                    Type        = (BonusType)ele["bonusType"].AsInt,
                    Value       = ele["bonusValue"].AsFloat,
                    UnlockLevel = ele["unlockLevel"].AsInt,
                };

                passives.Add(data);
            }

            return passives.ToArray();
        }
    }


    public class GameMercData
    {
        Dictionary<MercID, MercData> mercData;

        public GameMercData(JSONNode node)
        {
            LocalMercData[] local = LoadLocalData();

            mercData = new Dictionary<MercID, MercData>();

            foreach (LocalMercData desc in local)
                mercData[desc.ID] = new MercData(desc, node[(int)desc.ID]);
        }


        public MercData Get(MercID merc)
        {
            return mercData[merc];
        }


        LocalMercData[] LoadLocalData() => Resources.LoadAll<LocalMercData>("Mercs");
    }
}
