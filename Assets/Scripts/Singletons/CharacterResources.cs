using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CharacterID = CharacterData.CharacterID;

public class CharacterResources : MonoBehaviour
{
    public static CharacterResources Instance = null;

    // === Data ===
    public List<ScriptableCharacter> Characters;


    void Awake()
    {
        foreach (var c in Characters)
            c.Init();

        Instance = this;

        BigDouble prevPurchaseCost = 0;

        for (int i = 0; i < Characters.Count; i++)
        {
            var chara = Characters[i];

            chara.unlockOrder = i;

            if (chara.purchaseCost < prevPurchaseCost)
            {
                Debug.LogWarning("Hint: {0} | Character unlock order is incorrect (Purchase cost is not fully ascending)".Replace("{0}", chara.character.ToString()));

                Debug.Break();
            }

            prevPurchaseCost = chara.purchaseCost;
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
            if (!GameState.Characters.TryGetState(chara.character, out var _))
            {
                result = chara;

                return true;
            }
        }

        return false;
    }
}
