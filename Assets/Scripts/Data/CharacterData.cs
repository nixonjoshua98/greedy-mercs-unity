using System;
using System.Collections.Generic;

using UnityEngine;

using SimpleJSON;

namespace CharacterData
{
    public enum CharacterID
    {
        HERO_NUM_1 = 0,
        HERO_NUM_2 = 1,
        HERO_NUM_3 = 2,
        HERO_NUM_4 = 3,
        HERO_NUM_5 = 4,
        HERO_NUM_6 = 5,
    }

    [System.Serializable]
    public class HeroPassiveUnlock
    {
        public int skill;

        public int unlockLevel;
    }

    public class Characters
    {
        public Dictionary<CharacterID, List<HeroPassiveUnlock>> characters;

        public Characters(JSONNode node)
        {
            characters = new Dictionary<CharacterID, List<HeroPassiveUnlock>>();
            
            foreach (CharacterID chara in Enum.GetValues(typeof(CharacterID)))
            {
                characters[chara] = new List<HeroPassiveUnlock>();

                string key = ((int)chara).ToString();

                foreach (JSONNode passive in node[key]["passives"].AsArray)
                {
                    characters[chara].Add(JsonUtility.FromJson<HeroPassiveUnlock>(passive.ToString()));
                }
            }
        }

        public List<HeroPassiveUnlock> GetPassives(CharacterID chara)
        {
            return characters[chara];
        }
    }
}
