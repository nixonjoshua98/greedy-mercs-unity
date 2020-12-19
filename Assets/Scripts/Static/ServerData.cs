using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using SimpleJSON;
using System;

[System.Serializable]
public class HeroPassiveUnlock
{
    public int skill = -1;

    public int unlockLevel = 100_000;
}

[System.Serializable]
public class StaticServerHeroData
{
    public float baseCost = 1;
}


[System.Serializable]
public class HeroPassiveSkill
{
    public string name = "No Name";

    public double value = 1;

    public PassiveSkillType type = PassiveSkillType.ERROR;
}

public class BonusesFromHeroes
{
    public double allSquadDamage = 1;
}

public class ServerData
{
    static _ServerData Data = null;

    class _ServerData
    {
        public Dictionary<int, HeroPassiveSkill> heroPassiveSkills;

        public Dictionary<HeroID, List<HeroPassiveUnlock>> heroPassives;

        public Dictionary<HeroID, StaticServerHeroData> staticHeroData;

        public _ServerData(JSONNode json)
        {
            ParseHeroPassives(json);

            AssignHeroPassives(json);

            SetHeroStaticData(json);
        }

        void ParseHeroPassives(JSONNode parsedJson)
        {
            heroPassiveSkills = new Dictionary<int, HeroPassiveSkill>();

            JSONNode skillsJson = parsedJson["heroPassiveSkills"];

            foreach (string key in skillsJson.Keys)
            {
                HeroPassiveSkill skill = JsonUtility.FromJson<HeroPassiveSkill>(skillsJson[key].ToString());

                heroPassiveSkills.Add(int.Parse(key), skill);
            }
        }

        void AssignHeroPassives(JSONNode parsedJson)
        {
            heroPassives = new Dictionary<HeroID, List<HeroPassiveUnlock>>();

            JSONNode heroes = parsedJson["heroes"];

            foreach (HeroID hero in Enum.GetValues(typeof(HeroID)))
            {
                if ((int)hero >= 0)
                {
                    string jsonKey = ((int)hero).ToString();

                    heroPassives.Add(hero, new List<HeroPassiveUnlock>());

                    foreach (JSONNode skillString in heroes[jsonKey]["skills"])
                    {
                        HeroPassiveUnlock unlock = JsonUtility.FromJson<HeroPassiveUnlock>(skillString.ToString());

                        heroPassives[hero].Add(unlock);
                    }
                }
            }
        }

        void SetHeroStaticData(JSONNode parsedJson)
        {
            staticHeroData = new Dictionary<HeroID, StaticServerHeroData>();

            JSONNode heroes = parsedJson["heroes"];

            foreach (HeroID hero in Enum.GetValues(typeof(HeroID)))
            {
                if ((int)hero >= 0)
                {
                    string jsonKey = ((int)hero).ToString();

                    JSONNode staticData = heroes[jsonKey]["static"];

                    staticHeroData[hero] = JsonUtility.FromJson<StaticServerHeroData>(staticData.ToString());
                }
            }
        }
    }

    public static void Restore(string json)
    {
        if (Data == null)
            Data = new _ServerData(JSON.Parse(json));
    }   
    
    // === Helper Methods ===

    public static List<HeroPassiveUnlock> GetHeroPassiveSkills(HeroID hero)
    {
        if (Data.heroPassives.TryGetValue(hero, out List<HeroPassiveUnlock> ls))
            return ls;

        return new List<HeroPassiveUnlock>();
    }

    public static HeroPassiveSkill GetPassiveData(int skill)
    {
        return Data.heroPassiveSkills[skill];
    }

    public static StaticServerHeroData GetStaticHeroData(HeroID hero)
    {
        return Data.staticHeroData[hero];
    }

    public static BonusesFromHeroes GetBonusesFromHeroes()
    {
        BonusesFromHeroes bonuses = new BonusesFromHeroes();

        foreach (HeroID hero in Enum.GetValues(typeof(HeroID)))
        {
            HeroState heroState = GameState.GetHeroState(hero);

            List<HeroPassiveUnlock> heroPassiveUnlocks = GetHeroPassiveSkills(hero);

            foreach (HeroPassiveUnlock unlock in heroPassiveUnlocks)
            {
                if (heroState.level >= unlock.unlockLevel)
                {
                    HeroPassiveSkill skill = GetPassiveData(unlock.skill);

                    switch (skill.type)
                    {
                        case PassiveSkillType.ALL_SQUAD_DAMAGE:
                            bonuses.allSquadDamage *= skill.value;
                            break;
                    }
                }
            }
        }

        return bonuses;
    }
}
