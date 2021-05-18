
using System;

using System.Collections.Generic;

using SimpleJSON;

namespace GM.Armoury
{
    using Utils = GreedyMercs.Utils;

    using Formulas = GreedyMercs.Formulas;

    public class ArmouryItemState
    {
        public readonly int ItemID;

        public int level;
        public int owned;
        public int evoLevel;

        public ArmouryItemState(int itemId)
        {
            ItemID = itemId;
        }
    }

    public class ArmouryManager
    {
        public static ArmouryManager Instance = null;

        Dictionary<int, ArmouryItemState> states;

        public ArmouryManager()
        {
            states = new Dictionary<int, ArmouryItemState>();
        }

        public static ArmouryManager Create(JSONNode node)
        {
            Instance = new ArmouryManager();

            foreach (JSONNode item in node.AsArray)
            {
                int itemId = item["itemId"].AsInt;

                ArmouryItemState state = new ArmouryItemState(itemId) {
                    level       = item["level"].AsInt, 
                    owned       = item["owned"].AsInt,
                    evoLevel    = item["evoLevel"].AsInt
                };

                Instance.SetWeaponState(itemId, state);
            }

            return Instance;
        }


        // = = = Server Methods = = =
        public void UpgradeItem(int weaponIndex, Action<long, string> call)
        {
            void Callback(long code, string body)
            {
                if (code == 200)
                {
                    JSONNode returnNode = Utils.Json.Decompress(body);

                    int levelsGained = returnNode["levelsGained"].AsInt;

                    AddUpgradeLevel(weaponIndex, levelsGained);
                }

                call(code, body);
            }

            JSONNode node = Utils.Json.GetDeviceInfo();

            node["itemId"] = weaponIndex;

            Server.Put("armoury", "upgradeItem", node, Callback);
        }


        public void EvolveItem(int weaponIndex, Action<long, string> call)
        {
            void Callback(long code, string body)
            {
                if (code == 200)
                {
                    JSONNode returnNode = Utils.Json.Decompress(body);

                    int evoLevelsGained = returnNode["evoLevelsGained"];

                    AddEvolveLevel(weaponIndex, evoLevelsGained);
                }

                call(code, body);
            }

            JSONNode node = Utils.Json.GetDeviceInfo();

            node["itemId"] = weaponIndex;

            Server.Put("armoury", "evolveItem", node, Callback);
        }


        // 
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


        // = = = SET = = =
        void SetWeaponState(int itemId, ArmouryItemState state)
        {
            states[itemId] = state;
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
        public void AddUpgradeLevel(int index, int levels) { GetItem(index).level += levels; }
        public void AddOwnedItem(int index, int total) { GetItem(index).owned += total; }
        public void AddEvolveLevel(int index, int levels) { GetItem(index).evoLevel += levels; }
    }
}
