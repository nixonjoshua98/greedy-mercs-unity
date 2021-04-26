
using System;
using System.Collections.Generic;

using SimpleJSON;

namespace GM.Armoury
{
    using Utils = GreedyMercs.Utils;

    using Formulas = GreedyMercs.Formulas;

    public class ArmouryItemState
    {
        public readonly int WeaponIndex;

        public int level;
        public int owned;
        public int evoLevel;

        public ArmouryItemState(int index)
        {
            WeaponIndex = index;
        }
    }

    public class ArmouryManager
    {
        public static ArmouryManager Instance = null;

        Dictionary<int, ArmouryItemState> items;

        public ArmouryManager()
        {
            items = new Dictionary<int, ArmouryItemState>();
        }

        public static ArmouryManager Create(JSONNode playerData)
        {
            Instance = new ArmouryManager();

            JSONNode items = playerData;

            foreach (string key in items.Keys)
            {
                int weaponIndex = int.Parse(key);

                JSONNode item = items[key];

                ArmouryItemState state = new ArmouryItemState(weaponIndex)
                {
                    level       = item.GetValueOrDefault("level", 0),
                    owned       = item.GetValueOrDefault("owned", 0),
                    evoLevel    = item.GetValueOrDefault("evoLevel", 0)
                };

                Instance.SetWeaponState(weaponIndex, state);
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

                    int levelsGained = returnNode.GetValueOrDefault("levelsGained", 0);

                    UpgradeItem(weaponIndex, levelsGained);
                }

                call(code, body);
            }

            JSONNode node = Utils.Json.GetDeviceNode();

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

                    int evoLevelsGained = returnNode.GetValueOrDefault("evoLevelsGained", 0);

                    EvoUpgradeItem(weaponIndex, evoLevelsGained);
                }

                call(code, body);
            }

            JSONNode node = Utils.Json.GetDeviceNode();

            node["itemId"] = weaponIndex;

            Server.Put("armoury", "evolveItem", node, Callback);
        }


        // 
        public double DamageBonus()
        {
            double val = 0;

            foreach (var w in items)
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
            items[itemId] = state;
        }

        // = = = GET = = =
        public List<ArmouryItemState> GetOwned()
        {
            List<ArmouryItemState> ls = new List<ArmouryItemState>();

            foreach (ArmouryItemState state in items.Values)
            {
                if (state.owned > 0)
                    ls.Add(state);
            }

            return ls;
        }

        public ArmouryItemState GetItem(int index)
        {
            if (!items.ContainsKey(index))
                items[index] = new ArmouryItemState(index);

            return items[index];
        }

        // = = = Shorthand GET Methods = = = 
        public void UpgradeItem(int index, int levels) { GetItem(index).level += levels; }
        public void AddOwnedItem(int index, int total) { GetItem(index).owned += total; }
        public void EvoUpgradeItem(int index, int levels) { GetItem(index).evoLevel += levels; }
    }
}
