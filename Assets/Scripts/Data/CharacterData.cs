using System;
using System.Collections.Generic;

using UnityEngine;

using SimpleJSON;

namespace CharacterData
{
    public enum CharacterID
    {
        WRAITH      = 0,
        GOLEM       = 1,
        SATYR       = 2,
        ANGEL       = 3,
        MINOTAUR    = 4,
        REAPER      = 5,
        FIRE_GOLEM  = 6
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
