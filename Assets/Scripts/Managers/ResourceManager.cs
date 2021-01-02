using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CharacterID = CharacterData.CharacterID;

public class ResourceManager : MonoBehaviour
{
    static ResourceManager _instance = null;

    public static ResourceManager Instance { 
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
}
