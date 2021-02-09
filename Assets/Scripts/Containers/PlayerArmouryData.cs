using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GreedyMercs.Armoury.Data
{
    public class ArmouryWeaponState
    {
        public int level;
        public int owned;
        public int evoLevel;
    }

    public class PlayerArmouryData
    {
        Dictionary<int, ArmouryWeaponState> items;

        public PlayerArmouryData(JSONNode node)
        {
            Update(node);
        }

        public void Update(JSONNode node)
        {
            items = new Dictionary<int, ArmouryWeaponState>();

            foreach (string key in node.Keys)
            {
                JSONNode item = node[key];

                items.Add(int.Parse(key), JsonUtility.FromJson<ArmouryWeaponState>(item.ToString()));
            }
        }

        public JSONNode ToJson()
        {
            JSONNode node = new JSONObject();

            foreach (var entry in items)
            {
                node.Add(((int)entry.Key).ToString(), JSON.Parse(JsonUtility.ToJson(entry.Value)));
            }

            return node;
        }

        public ArmouryWeaponState GetWeapon(int index)
        {
            if (!items.ContainsKey(index))
            {
                items[index] = new ArmouryWeaponState { level = 0 };
            }

            return items[index];
        }

        public double DamageBonus()
        {
            double val = 0;

            foreach (var w in items)
            {
                ArmouryWeaponState state = GameState.Armoury.GetWeapon(w.Key);

                if (state.level > 0)
                    val += Formulas.Armoury.WeaponDamage(w.Key);
            }

            return val;
        }

        public void UpgradeWeapon(int index, int levels) => GetWeapon(index).level += levels;

        public void AddItem(int index) => GetWeapon(index).owned += 1;
    }
}
