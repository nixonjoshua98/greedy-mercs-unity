
using System;

using System.Collections.Generic;

using SimpleJSON;

namespace GM.Armoury
{
    using Utils = GM.Utils;

    using Formulas = GM.Formulas;

    public class ArmouryItemState
    {
        public int ID;

        public int level;
        public int owned;
        public int evoLevel;

        public ArmouryItemState(int itemId)
        {
            ID = itemId;
        }
    }

    public class ArmouryManager : IBonusManager
    {
        public static ArmouryManager Instance = null;

        Dictionary<int, ArmouryItemState> states;

        public static ArmouryManager Create(JSONNode node)
        {
            Instance = new ArmouryManager();

            Instance.SetArmouryItems(node);

            return Instance;
        }


        // = = = Server Methods = = =
        public void UpgradeItem(int itemId, Action<long, string> call)
        {
            void Callback(long code, string body)
            {
                if (code == 200)
                {
                    JSONNode resp = Utils.Json.Decompress(body);

                    int levelsGained = resp["levelsGained"].AsInt;

                    AddUpgradeLevel(itemId, levelsGained);
                }

                call(code, body);
            }

            Server.Put("armoury", "upgradeItem", CreateJson(itemId), Callback);
        }


        public void EvolveItem(int itemId, Action<long, string> call)
        {
            void Callback(long code, string body)
            {
                if (code == 200)
                {
                    JSONNode resp = Utils.Json.Decompress(body);

                    int evoLevelsGained = resp["evoLevelsGained"];

                    AddEvolveLevel(itemId, evoLevelsGained);
                }

                call(code, body);
            }

            Server.Put("armoury", "evolveItem", CreateJson(itemId), Callback);
        }


        // = = = GET = = =
        public List<ArmouryItemState> GetOwned()
        {
            List<ArmouryItemState> ls = new List<ArmouryItemState>();

            foreach (ArmouryItemState state in states.Values)
            {
                if (state.owned > 0)
                    ls.Add(state);
            }

            return ls;
        }

        public ArmouryItemState GetItem(int index)
        {
            if (!states.ContainsKey(index))
                states[index] = new ArmouryItemState(index);

            return states[index];
        }

        // = = = Shorthand GET Methods = = = 
        void AddUpgradeLevel(int index, int levels) { GetItem(index).level += levels; }
        void AddEvolveLevel(int index, int levels) { GetItem(index).evoLevel += levels; }

        public void SetArmouryItems(JSONNode node)
        {
            states = new Dictionary<int, ArmouryItemState>();

            foreach (JSONNode item in node.AsArray)
            {
                int itemId = item["itemId"].AsInt;

                ArmouryItemState state = new ArmouryItemState(itemId)
                {
                    level = item["level"].AsInt,
                    owned = item["owned"].AsInt,
                    evoLevel = item["evoLevel"].AsInt
                };

                states[itemId] = state;
            }
        }

        // = = = Internal = = =
        JSONNode CreateJson(int itemId)
        {
            JSONNode node = Utils.Json.GetDeviceInfo();

            node["itemId"] = itemId;

            return node;
        }

        // = = = Special Methods = = = //
        public double DamageBonus()
        {
            double val = 0;

            foreach (var w in states)
            {
                ArmouryItemState state = GetItem(w.Key);

                if (state.level > 0)
                    val += Formulas.Armoury.WeaponDamage(w.Key);
            }

            return val;
        }

        public List<KeyValuePair<BonusType, double>> Bonuses()
        {
            double dmgBonus = DamageBonus();

            dmgBonus = dmgBonus > 0 ? dmgBonus : 1;

            List<KeyValuePair<BonusType, double>> ls = new List<KeyValuePair<BonusType, double>>
            {
                new KeyValuePair<BonusType, double>(BonusType.MERC_DAMAGE, dmgBonus)
            };

            return ls;
        }
    }
}
