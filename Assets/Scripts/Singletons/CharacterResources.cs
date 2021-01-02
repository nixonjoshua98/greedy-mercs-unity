using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CharacterID = CharacterData.CharacterID;

public class CharacterResources : MonoBehaviour
{
    static CharacterResources _instance = null;

    public static CharacterResources Instance { 
        get
        {
            if (_instance == null)
            {
                Debug.LogError("ResourceManager.Instance is null");

                Debug.Break();
            }

            return _instance;
        } 
    }

    // === Data ===
    public List<ScriptableCharacter> Characters;


    void Awake()
    {
        _instance = this;

        for (int i = 0; i < Characters.Count; i++)
        {
            Characters[i].unlockOrder = i;
        }
    }

    // === Helper Methods ===
    public ScriptableCharacter GetCharacter(CharacterID chara)
    {
        foreach (ScriptableCharacter scriptableChar in Characters)
        {
            if (scriptableChar.character == chara)
                return scriptableChar;
        }

        return null;
    }

    public static bool GetNextHero(out ScriptableCharacter result)
    {
        result = null;

        foreach (ScriptableCharacter chara in Instance.Characters)
        {
            if (!GameState.Characters.TryGetHeroState(chara.character, out var _))
            {
                result = chara;

                return true;
            }
        }

        return false;
    }
}
