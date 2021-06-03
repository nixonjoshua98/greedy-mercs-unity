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

        // Private attributes
        string _iconString;
        string _prefabString;
        // = = = = = = = = =

        public Sprite Icon { get { return ResourceManager.LoadSprite(_iconString); } }
        public GameObject Prefab { get { return ResourceManager.LoadPrefab(_prefabString); } }

        public static MercData FromJson(JSONNode node)
        {
            return new MercData()
            {
                Name = node["name"],

                AttackType = (BonusType)node["attackType"].AsInt,
                BaseDamage = new BigDouble(node["baseDamage"]),
                UnlockCost = node["unlockCost"],

                Passives = ParsePassives(node["passives"].AsArray),

                _prefabString   = node["prefabString"],
                _iconString     = node["iconString"],
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

                MercData inst = MercData.FromJson(current);

                mercs[id] = inst;
            }
        }
    }
}