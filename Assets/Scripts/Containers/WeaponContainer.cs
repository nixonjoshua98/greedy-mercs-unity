using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

using SimpleJSON;

namespace GreedyMercs
{

    public class WeaponContainer
    {
        Dictionary<CharacterID, Dictionary<int, int>> weapons;

        public WeaponContainer(JSONNode node)
        {
            Update(node);
        }

        public void Update(JSONNode node)
        {
            weapons = new Dictionary<CharacterID, Dictionary<int, int>>();

            foreach (string stringKey in node.Keys)
            {
                CharacterID key = (CharacterID)int.Parse(stringKey);

                weapons[key] = new Dictionary<int, int>();

                foreach (string weaponString in node[stringKey].Keys)
                {
                    weapons[key][int.Parse(weaponString)] = node[stringKey][weaponString].AsInt;
                }
            }
        }

        public JSONNode ToJson()
        {
            JSONNode node = new JSONObject();

            foreach (var entry in weapons)
            {
                JSONNode weaponNode = new JSONObject();

                foreach (var weapon in entry.Value)
                {
                    weaponNode.Add(weapon.Key.ToString(), weapon.Value);
                }

                node.Add(((int)entry.Key).ToString(), weaponNode);
            }

            return node;
        }

        public double CalcBonuses(CharacterID character)
        {
            double bonus = 1.0f;

            foreach (KeyValuePair<int, int> weapon in Get(character))
            {
                if (weapon.Value > 0)
                    bonus *= Formulas.CalcWeaponDamageMultiplier(weapon.Key, weapon.Value);
            }

            return bonus;
        }


        // === Helper Methods ===
        public Dictionary<int, int> Get(CharacterID chara)
        {
            if (!weapons.ContainsKey(chara))
                weapons[chara] = new Dictionary<int, int>();

            return weapons[chara];
        }

        public int Get(CharacterID chara, int index)
        {
            return Get(chara).GetOrVal(index, 0);
        }

        public int GetHighestTier(CharacterID chara)
        {
            var characterWeapons = Get(chara);

            var keys = characterWeapons.Keys.ToList();

            keys.Sort((a, b) => b.CompareTo(a));

            foreach (int key in keys)
            {
                if (characterWeapons[key] > 0)
                    return key;
            }

            return 0;
        }

        public void Add(CharacterID chara, int index, int amount)
        {
            weapons[chara][index] = Get(chara).GetOrVal(index, 0) + amount;
        }
    }
}