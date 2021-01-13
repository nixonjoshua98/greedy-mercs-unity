using System;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using SkillData;
using SimpleJSON;

public class SkillState
{
    SkillID SkillID;

    public DateTime skillActivated;

    public SkillState(SkillID skill)
    {
        SkillID = skill;
    }

    public bool IsActive { 
        get 
        {
            SkillSO scriptable = StaticData.Skills.Get(SkillID);

            return TimeSinceActivated <= scriptable.Duration; 
        }
    }

    public double TimeSinceActivated { get { return (DateTime.UtcNow - skillActivated).TotalSeconds; } }

    public void Activate()
    {
        skillActivated = DateTime.UtcNow;
    }
}



public class SkillsState
{
    Dictionary<SkillID, SkillState> skills;

    public SkillsState(JSONNode node)
    {
        skills = new Dictionary<SkillID, SkillState>();

        node = node["skills"];

        foreach (string key in node.Keys)
        {
            SkillID skill = (SkillID)int.Parse(key);

            SkillState state = new SkillState(skill);

            skills.Add(skill, state);
        }
    }

    public JSONNode ToJson()
    {
        JSONNode node = new JSONObject();

        foreach (var entry in skills)
        {
            JSONNode skillNode = new JSONObject();

            node.Add(((int)entry.Key).ToString(), skillNode);
        }

        return node;
    }

    public Dictionary<BonusType, double> CacBonuses()
    {
        Dictionary<BonusType, double> bonuses = new Dictionary<BonusType, double>();
        
        foreach (var entry in skills)
        {
            if (entry.Value.IsActive)
            {
                SkillSO data = StaticData.Skills.Get(entry.Key);

                bonuses[data.bonusType] = bonuses.GetOrVal(data.bonusType, 1) * data.bonusValue;
            }
        }

        return bonuses;
    }


    public void UnlockSkill(SkillID skill)
    {
        skills.Add(skill, new SkillState(skill));
    }

    public bool IsUnlocked(SkillID skill) => skills.ContainsKey(skill);

    public SkillState Get(SkillID skill) => skills[skill];

    public void ActivateSkill(SkillID skill)
    {
        SkillState state = Get(skill);

        state.Activate();
    }
}
