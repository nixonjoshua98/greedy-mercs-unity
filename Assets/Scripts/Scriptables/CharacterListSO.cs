using System.Collections.Generic;
using UnityEngine;

using SimpleJSON;

namespace CharacterData
{
    [PreferBinarySerialization]
    [CreateAssetMenu(menuName = "Scriptables/Container/CharacterList")]
    public class CharacterListSO : ScriptableObject
    {
        public List<CharacterSO> CharacterList;

        public void Restore(JSONNode node)
        {
            for (int i = 0; i < CharacterList.Count; ++i)
            {
                CharacterSO chara = CharacterList[i];

                string key = ((int)chara.CharacterID).ToString();

                List<HeroPassiveUnlock> passives = new List<HeroPassiveUnlock>();

                foreach (JSONNode passive in node[key]["passives"].AsArray)
                {
                    passives.Add(JsonUtility.FromJson<HeroPassiveUnlock>(passive.ToString()));
                }

                chara.OnAwake();

                chara.Init(i, passives);
            }

            CharacterList.Sort((x, y) => x.purchaseCost.CompareTo(y.purchaseCost));
        }

        // === Helper Methods ===

        public CharacterSO Get(CharacterID chara)
        {
            foreach (CharacterSO scriptableChar in CharacterList)
            {
                if (scriptableChar.CharacterID == chara)
                    return scriptableChar;
            }

            return null;
        }

        public bool GetNextHero(out CharacterSO result)
        {
            result = null;

            foreach (CharacterSO chara in CharacterList)
            {
                if (!GameState.Characters.Contains(chara.CharacterID))
                {
                    result = chara;

                    return true;
                }
            }

            return false;
        }
    }
}