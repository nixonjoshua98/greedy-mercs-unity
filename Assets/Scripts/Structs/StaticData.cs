using System;
using System.Collections.Generic;

using UnityEngine;

using SimpleJSON;


public class StaticData
{
    static _StaticData Data = null;

    public static int numRelics { get { return Data.relics.Count; } }

    public static void Restore(JSONNode json)
    {
        if (Data == null)
            Data = new _StaticData(json);
    }

    // === Helper Methods ===

    public static RelicStaticData GetRelic(RelicID relic) => Data.relics[relic];
    public static HeroPassiveSkill GetPassive(int skill) => Data.allHeroPassives[skill];
    public static List<HeroPassiveUnlock> GetCharPassives(CharacterID hero)  => Data.characterPassives[hero];

    public static Dictionary<BonusType, double> GetBonusesFromHeroes()
    {
        Dictionary<BonusType, double> bonuses = new Dictionary<BonusType, double>();

        foreach (CharacterID hero in Enum.GetValues(typeof(CharacterID)))
        {
            if (GameState.Characters.TryGetHeroState(hero, out var state))
            {
                List<HeroPassiveUnlock> heroPassiveUnlocks = GetCharPassives(hero);

                foreach (HeroPassiveUnlock unlock in heroPassiveUnlocks)
                {
                    if (state.level >= unlock.unlockLevel)
                    {
                        HeroPassiveSkill skill = GetPassive(unlock.skill);

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
        public Dictionary<int, HeroPassiveSkill> allHeroPassives;

        public Dictionary<CharacterID, List<HeroPassiveUnlock>> characterPassives;

        public Dictionary<RelicID, RelicStaticData> relics;

        public _StaticData(JSONNode json)
        {
            ParseHeroPassives(json);

            ParseRelics(json);

            AssignHeroPassives(json);
        }

        void ParseRelics(JSONNode node)
        {
            relics = new Dictionary<RelicID, RelicStaticData>();

            foreach (RelicID relic in Enum.GetValues(typeof(RelicID)))
            {
                JSONNode relicNode = node["relics"][((int)relic).ToString()];

                relics[relic] = JsonUtility.FromJson<RelicStaticData>(relicNode.ToString());
            }
        }

        void ParseHeroPassives(JSONNode parsedJson)
        {
            allHeroPassives = new Dictionary<int, HeroPassiveSkill>();

            JSONNode passives = parsedJson["characterPassives"];

            foreach (string key in passives.Keys)
                allHeroPassives[int.Parse(key)] = JsonUtility.FromJson<HeroPassiveSkill>(passives[key].ToString());
        }

        void AssignHeroPassives(JSONNode parsedJson)
        {
            characterPassives = new Dictionary<CharacterID, List<HeroPassiveUnlock>>();

            JSONNode characters = parsedJson["characters"];

            foreach (CharacterID hero in Enum.GetValues(typeof(CharacterID)))
            {
                characterPassives[hero] = new List<HeroPassiveUnlock>();

                foreach (JSONNode skillString in characters[((int)hero).ToString()]["passives"])
                    characterPassives[hero].Add(JsonUtility.FromJson<HeroPassiveUnlock>(skillString.ToString()));
            }
        }
    }
}
