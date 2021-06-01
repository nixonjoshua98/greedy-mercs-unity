using SimpleJSON;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Characters
{
    public struct MercPassiveData
    {
        public BonusType Type;
        public float Value;

        public int UnlockLevel;
    }

    public struct MercData
    {
        public string Name;

        public BonusType AttackType;

        public double UnlockCost;

        public BigDouble BaseDamage;

        public MercPassiveData[] Passives;
    }

    public class ServerMercData
    {
        Dictionary<CharacterID, MercData> mercs;

        public ServerMercData(JSONNode node)
        {
            ParseNode(node);
        }

        public MercData GetMerc(CharacterID merc)
        {
            return mercs[merc];
        }


        void ParseNode(JSONNode node)
        {
            mercs = new Dictionary<CharacterID, MercData>();

            foreach (string key in node.Keys)
            {
                JSONNode current = node[key];

                CharacterID id = (CharacterID)int.Parse(key);

                MercData inst = new MercData()
                {
                    Name = current["name"],
                    BaseDamage = new BigDouble(current["baseDamage"]),
                    AttackType = (BonusType)current["attackType"].AsInt,
                    UnlockCost = current["unlockCost"],
                    Passives = ParsePassives(current["passives"].AsArray)
                };

                mercs[id] = inst;
            }
        }

        MercPassiveData[] ParsePassives(JSONArray node)
        {
            List<MercPassiveData> passives = new List<MercPassiveData>();

            foreach (JSONNode passive in node.AsArray)
            {
                MercPassiveData data = new MercPassiveData()
                {
                    Type = (BonusType)passive["bonusType"].AsInt,
                    Value = passive["bonusValue"].AsFloat,
                    UnlockLevel = passive["unlockLevel"].AsInt
                };

                passives.Add(data);
            }

            return passives.ToArray();
        }
    }
}