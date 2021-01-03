﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SimpleJSON;

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

    public void Add(CharacterID chara, int index, int amount)
    {
        weapons[chara][index] = Get(chara).GetValueOrDefault(index, 0) + amount;
    }
}
