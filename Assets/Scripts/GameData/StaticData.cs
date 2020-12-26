﻿using System;
using System.Collections.Generic;

using UnityEngine;

using SimpleJSON;


public class StaticData
{
    static _StaticData Data = null;

    public static void Restore(JSONNode json)
    {
        if (Data == null)
            Data = new _StaticData(json);
    }

    // === Helper Methods ===

    public static List<HeroPassiveUnlock> GetHeroPassiveSkills(CharacterID hero)
    {
        if (Data.heroPassives.TryGetValue(hero, out List<HeroPassiveUnlock> ls))
            return ls;

        return new List<HeroPassiveUnlock>();
    }

    public static HeroPassiveSkill GetPassiveData(int skill)
    {
        return Data.heroPassiveSkills[skill];
    }

    public static Dictionary<BonusType, double> GetBonusesFromHeroes()
    {
        Dictionary<BonusType, double> bonuses = new Dictionary<BonusType, double>();

        foreach (CharacterID hero in Enum.GetValues(typeof(CharacterID)))
        {
            if (GameState.TryGetHeroState(hero, out var state))
            {
                List<HeroPassiveUnlock> heroPassiveUnlocks = GetHeroPassiveSkills(hero);

                foreach (HeroPassiveUnlock unlock in heroPassiveUnlocks)
                {
                    if (state.level >= unlock.unlockLevel)
                    {
                        HeroPassiveSkill skill = GetPassiveData(unlock.skill);

                        bonuses[skill.bonusType] = bonuses.GetValueOrDefault(skill.bonusType, 1) * skill.value;
                    }
                }
            }
        }

        return bonuses;
    }

    // ===

    class _StaticData
    {
        public Dictionary<int, HeroPassiveSkill> heroPassiveSkills;

        public Dictionary<CharacterID, List<HeroPassiveUnlock>> heroPassives;

        public _StaticData(JSONNode json)
        {
            ParseHeroPassives(json);

            AssignHeroPassives(json);
        }

        void ParseHeroPassives(JSONNode parsedJson)
        {
            heroPassiveSkills = new Dictionary<int, HeroPassiveSkill>();

            JSONNode passives = parsedJson["characterPassives"];

            foreach (string key in passives.Keys)
            {
                heroPassiveSkills[int.Parse(key)] = JsonUtility.FromJson<HeroPassiveSkill>(passives[key].ToString());
            }
        }

        void AssignHeroPassives(JSONNode parsedJson)
        {
            heroPassives = new Dictionary<CharacterID, List<HeroPassiveUnlock>>();

            JSONNode characters = parsedJson["characters"];

            foreach (CharacterID hero in Enum.GetValues(typeof(CharacterID)))
            {
                if ((int)hero >= 0)
                {
                    string jsonKey = ((int)hero).ToString();

                    heroPassives[hero] = new List<HeroPassiveUnlock>();

                    foreach (JSONNode skillString in characters[jsonKey]["passives"])
                    {
                        heroPassives[hero].Add(JsonUtility.FromJson<HeroPassiveUnlock>(skillString.ToString()));
                    }
                }
            }
        }
    }
}
