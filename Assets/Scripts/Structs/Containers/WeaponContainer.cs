using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

using SimpleJSON;

using WeaponData;

using CharacterID = CharacterData.CharacterID;

public class WeaponContainer
{
    Dictionary<CharacterID, Dictionary<int, int>> weapons;

    public WeaponContainer(JSONNode node)
    {
        Update(node);
    }

    public void Update(JSONNode node)
    {
        if (node.HasKey("weapons"))
        {
            weapons = new Dictionary<CharacterID, Dictionary<int, int>>();

            foreach (string stringKey in node["weapons"].Keys)
            {
                CharacterID key = (CharacterID)int.Parse(stringKey);

                weapons[key] = new Dictionary<int, int>();

                foreach (string weaponString in node["weapons"][stringKey].Keys)
                {
                    weapons[key][int.Parse(weaponString)] = node["weapons"][stringKey][weaponString].AsInt;
                }
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

    public double CalculateDamageBonus(CharacterID character)
    {
        double bonus = 1.0f;

        foreach (KeyValuePair<int, int> weapon in Get(character))
        {
            WeaponStaticData staticData = StaticData.Weapons.GetWeaponAtIndex(weapon.Key);

            // Weapon owned > 0
            if (weapon.Value > 0)
                bonus *= (weapon.Value * staticData.damageBonus);
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
        return Get(chara).GetValueOrDefault(index, 0);
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
        weapons[chara][index] = Get(chara).GetValueOrDefault(index, 0) + amount;
    }
}
