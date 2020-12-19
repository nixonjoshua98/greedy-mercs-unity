using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using SimpleJSON;
using System;

[System.Serializable]
public class HeroPassiveUnlock
{
    public PassiveSkillID skill = PassiveSkillID.ERROR;

    public int unlockLevel = 100_000;
}

[System.Serializable]
public class HeroPassiveSkill
{
    public string name = "No Name";
    public string desc = "No Description";

    public double value = 1;

    public PassiveSkillType type = PassiveSkillType.ERROR;
}

public class ServerData
{
    static _ServerData Data = null;

    class _ServerData
    {
        public Dictionary<PassiveSkillID, HeroPassiveSkill> heroPassiveSkills;

        public Dictionary<HeroID, List<HeroPassiveUnlock>> heroPassives;

        public _ServerData(JSONNode json)
        {
            ParseHeroPassives(json);

            AssignHeroPassives(json);
        }

        void ParseHeroPassives(JSONNode parsedJson)
        {
            heroPassiveSkills = new Dictionary<PassiveSkillID, HeroPassiveSkill>();

            JSONNode skillsJson = parsedJson["heroPassiveSkills"];

            foreach (PassiveSkillID passive in Enum.GetValues(typeof(PassiveSkillID)))
            {
                if ((int)passive > 0)
                {
                    string jsonKey = ((int)passive).ToString();

                    HeroPassiveSkill skill = JsonUtility.FromJson<HeroPassiveSkill>(skillsJson[jsonKey].ToString());

                    heroPassiveSkills.Add(passive, skill);
                }
            }
        }

        void AssignHeroPassives(JSONNode parsedJson)
        {
            heroPassives = new Dictionary<HeroID, List<HeroPassiveUnlock>>();

            JSONNode heroes = parsedJson["heroes"];

            foreach (HeroID hero in Enum.GetValues(typeof(HeroID)))
            {
                if ((int)hero > 0)
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

    public static HeroPassiveSkill GetPassiveData(PassiveSkillID skill)
    {
        return Data.heroPassiveSkills[skill];
    }
}
