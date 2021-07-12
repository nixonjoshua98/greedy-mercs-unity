using SimpleJSON;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Units
{
    public struct MercPassiveData
    {
        public BonusType Type;
        public float Value;

        public int UnlockLevel;
    }

    public struct MercData
    {
        public BonusType AttackType;

        public double UnlockCost;

        public BigDouble BaseDamage;

        public MercPassiveData[] Passives;

        public static MercData FromJson(JSONNode node)
        {
            return new MercData()
            {
                AttackType = (BonusType)node["attackType"].AsInt,
                BaseDamage = new BigDouble(node["baseDamage"]),
                UnlockCost = node["unlockCost"],

                Passives = ParsePassives(node["passives"].AsArray),
            };
        }

        static MercPassiveData[] ParsePassives(JSONArray node)
        {
            List<MercPassiveData> passives = new List<MercPassiveData>();

            foreach (JSONNode passive in node.AsArray)
            {
                MercPassiveData data = new MercPassiveData()
                {
                    Type        = (BonusType)passive["bonusType"].AsInt,
                    Value       = passive["bonusValue"].AsFloat,
                    UnlockLevel = passive["unlockLevel"].AsInt,
                };

                passives.Add(data);
            }

            return passives.ToArray();
        }
    }

    public class ServerMercData
    {
        Dictionary<MercID, MercData> mercs;

        public ServerMercData(JSONNode node)
        {
            ParseNode(node);
        }

        public MercData GetMerc(MercID merc)
        {
            return mercs[merc];
        }


        void ParseNode(JSONNode node)
        {
            mercs = new Dictionary<MercID, MercData>();

            foreach (string key in node.Keys)
            {
                JSONNode current = node[key];

                MercID id = (MercID)int.Parse(key);

                MercData inst = MercData.FromJson(current);

                mercs[id] = inst;
            }
        }
    }
}