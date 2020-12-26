using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterContainer : IContainer
{
    Dictionary<CharacterID, UpgradeState> characters;

    public CharacterContainer(JSONNode node)
    {
        characters = new Dictionary<CharacterID, UpgradeState>();

        foreach (JSONNode chara in node["characters"].AsArray)
        {
            characters[(CharacterID)int.Parse(chara["characterId"])] = JsonUtility.FromJson<UpgradeState>(chara.ToString());
        }
    }

    public JSONNode ToJson()
    {
        return Utils.Json.CreateJSONArray("characterId", characters);
    }

    // === Helper Methods ===

    public UpgradeState GetCharacter(CharacterID chara) 
    { 
        return characters[chara]; 
    }

    public bool TryGetHeroState(CharacterID chara, out UpgradeState result) 
    {
        return characters.TryGetValue(chara, out result);
    }

    public void AddHero(CharacterID charaId)
    {
        characters[charaId] = new UpgradeState { level = 1 };
    }
}
