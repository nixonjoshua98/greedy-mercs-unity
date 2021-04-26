
using System.Linq;
using System.Collections.Generic;

using SimpleJSON;

namespace GreedyMercs.Armoury.Data
{
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

    public class PlayerArmouryData
    {
        Dictionary<int, ArmouryItemState> items;

        public PlayerArmouryData()
        {
            items = new Dictionary<int, ArmouryItemState>();
        }

        public static PlayerArmouryData FromJsonNode(JSONNode node)
        {
            PlayerArmouryData inst = new PlayerArmouryData();

            JSONNode items = node;

            foreach (string key in items.Keys)
            {
                int weaponIndex = int.Parse(key);

                JSONNode item = items[key];

                ArmouryItemState state = new ArmouryItemState(weaponIndex)
                {
                    level = item.GetValueOrDefault("level", 0),
                    owned = item.GetValueOrDefault("owned", 0),
                    evoLevel = item.GetValueOrDefault("evoLevel", 0)
                };

                inst.SetWeaponState(weaponIndex, state);
            }

            return inst;
        }

        public JSONNode Serialize()
        {
            return Funcs.SerializeDictionary(items);
        }

        public double DamageBonus()
        {
            double val = 0;

            foreach (var w in items)
            {
                ArmouryItemState state = GameState.Armoury.GetItem(w.Key);

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
                {
                    ls.Add(state);
                }
            }

            return ls;

        }

        public ArmouryItemState GetItem(int index)
        {
            if (!items.ContainsKey(index))
                items[index] = new ArmouryItemState(index);

            return items[index];
        }

        public void UpgradeItem(int index, int levels)
        {
            GetItem(index).level += levels;
        }

        public void EvoUpgradeItem(int index, int levels)
        {
            GetItem(index).evoLevel += levels;
        }


        public void AddOwnedItem(int index, int total = 1)
        {
            GetItem(index).owned += total;
        }
    }
}
