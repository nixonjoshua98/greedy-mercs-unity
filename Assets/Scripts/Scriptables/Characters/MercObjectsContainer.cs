using System.Collections.Generic;
using UnityEngine;

using SimpleJSON;

namespace GreedyMercs
{
    using GM.Characters;

    [PreferBinarySerialization]
    [CreateAssetMenu(menuName = "Scriptables/MercObjectsContainer")]
    public class MercObjectsContainer : ScriptableObject
    {
        [SerializeField] public MercContainer[] mercsArray;

        // === Helper Methods ===

        public MercContainer Get(CharacterID chara)
        {
            foreach (MercContainer scriptableChar in mercsArray)
            {
                if (scriptableChar.ID == chara)
                    return scriptableChar;
            }

            return null;
        }

        public bool GetNextHero(out CharacterID result)
        {
            result = (CharacterID)(-1);

            foreach (MercContainer chara in mercsArray)
            {
                if (!GameState.Characters.Contains(chara.ID))
                {
                    result = chara.ID;

                    return true;
                }
            }

            return false;
        }
    }
}