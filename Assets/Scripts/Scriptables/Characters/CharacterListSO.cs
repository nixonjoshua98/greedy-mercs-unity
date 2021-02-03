using System.Collections.Generic;
using UnityEngine;

using SimpleJSON;

namespace GreedyMercs
{
    public struct CharacterPassive
    {
        public BonusType bonusType;

        public double value;

        public int unlockLevel;

        public CharacterPassive Clone() => new CharacterPassive { bonusType = bonusType, value = value, unlockLevel = unlockLevel };

        public override string ToString() => (value * 100).ToString() + "% " + Utils.Generic.BonusToString(bonusType);
    }

    [PreferBinarySerialization]
    [CreateAssetMenu(menuName = "Scriptables/Container/CharacterList")]
    public class CharacterListSO : ScriptableObject
    {
        public List<CharacterSO> CharacterList;

        public void Init(JSONNode characters, JSONNode passivesData)
        {
            Dictionary<int, CharacterPassive> passiveDict = GetPassiveDict(passivesData);

            for (int i = 0; i < CharacterList.Count; ++i)
            {
                CharacterSO chara = CharacterList[i];

                string key = ((int)chara.CharacterID).ToString();

                List<CharacterPassive> passives = new List<CharacterPassive>();

                JSONNode charaNode = characters[key];

                foreach (JSONNode passive in charaNode["passives"].AsArray)
                {
                    CharacterPassive p = passiveDict[passive["skill"].AsInt].Clone();

                    p.unlockLevel = passive["unlockLevel"].AsInt;

                    passives.Add(p);
                }

                chara.Init(i, charaNode["unlockCost"].Value, charaNode["baseDamage"].Value, passives);
            }

            CharacterList.Sort((x, y) => x.unlockCost.CompareTo(y.unlockCost));
        }

        Dictionary<int, CharacterPassive> GetPassiveDict(JSONNode node)
        {
            Dictionary<int, CharacterPassive> passives = new Dictionary<int, CharacterPassive>();

            foreach (string key in node.Keys)
            {
                passives[int.Parse(key)] = JsonUtility.FromJson<CharacterPassive>(node[key].ToString());
            }

            return passives;
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