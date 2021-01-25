using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GreedyMercs.Armoury.Data
{
    public class ArmouryWeaponState
    {
        public int level;
    }

    public class PlayerArmouryData
    {
        Dictionary<int, ArmouryWeaponState> weapons;

        public PlayerArmouryData(JSONNode node)
        {
            Update(node);
        }

        public void Update(JSONNode node)
        {
            weapons = new Dictionary<int, ArmouryWeaponState>();

            foreach (string key in node.Keys)
            {
                weapons.Add(int.Parse(key), new ArmouryWeaponState { level = node[key].AsInt });
            }
        }

        public ArmouryWeaponState GetWeapon(int index)
        {
            if (!weapons.TryGetValue(index, out ArmouryWeaponState state))
            {
                weapons[index] = new ArmouryWeaponState { level = 0 };
            }

            return weapons[index];
        }

        public double DamageBonus()
        {
            double val = 1;

            foreach (var w in weapons)
            {
                ArmouryWeaponState state = GameState.Armoury.GetWeapon(w.Key);

                if (state.level > 0)
                {
                    ArmouryItemSO scriptable = StaticData.Armoury.GetWeapon(w.Key);

                    val *= 1 + ((scriptable.damageBonus) - 1) * state.level;
                }
            }

            return val > 1 ? val : 0;
        }

        public double DamageBonus(int index)
        {
            ArmouryWeaponState state = GameState.Armoury.GetWeapon(index);

            if (state.level > 0)
            {
                ArmouryItemSO scriptable = StaticData.Armoury.GetWeapon(index);

                return 1 + ((scriptable.damageBonus) - 1) * state.level;
            }

            return 0;
        }

        public double DamageBonus(int index, int level)
        {
            ArmouryItemSO scriptable = StaticData.Armoury.GetWeapon(index);

            double val = 1 + ((scriptable.damageBonus) - 1) * level;

            return val > 1 ? val : 0;
        }
    }
}
