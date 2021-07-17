using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using SimpleJSON;

namespace GM.Data
{
    public enum AttackType
    {
        MELEE = 0,
        RANGED = 1
    }


    public struct MercPassiveData
    {
        public BonusType Type;

        public float Value;

        public int UnlockLevel;
    }


    public struct MercData
    {
        public MercID Id;
        public AttackType Attack;

        public string Name;

        public double UnlockCost;
        public double BaseDamage;

        public MercPassiveData[] Passives;

        public GameObject Prefab;
        public Sprite Icon;
    }


    public class GameMercData
    {
        Dictionary<MercID, MercData> mercData;

        public GameMercData(JSONNode node)
        {
            LocalMercData[] local = LoadLocalData();

            mercData = new Dictionary<MercID, MercData>();

            foreach (LocalMercData desc in local)
            {
                JSONNode current = node[(int)desc.ID];

                mercData[desc.ID] = new MercData()
                {
                    Id = desc.ID,
                    Name = desc.Name,

                    Icon=desc.Icon,
                    Prefab=desc.Prefab,

                    Attack = (AttackType)current["attackType"].AsInt,
                    BaseDamage = current["baseDamage"].AsDouble,
                    UnlockCost = current["unlockCost"].AsDouble,

                    Passives = ParsePassives(current["passives"].AsArray),
                };
            }
        }


        public MercData Get(MercID merc)
        {
            return mercData[merc];
        }


        MercPassiveData[] ParsePassives(JSONArray arr)
        {
            List<MercPassiveData> passives = new List<MercPassiveData>();

            foreach (JSONNode element in arr)
            {
                MercPassiveData data = new MercPassiveData()
                {
                    Type = (BonusType)element["bonusType"].AsInt,
                    Value = element["bonusValue"].AsFloat,
                    UnlockLevel = element["unlockLevel"].AsInt,
                };

                passives.Add(data);
            }

            return passives.ToArray();
        }


        LocalMercData[] LoadLocalData() => Resources.LoadAll<LocalMercData>("Mercs");
    }
}
