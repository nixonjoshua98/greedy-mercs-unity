using System;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using SkillData;

public class SkillState
{
    SkillID SkillID;

    DateTime SkillActivated;

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

    public double TimeSinceActivated { get { return (DateTime.UtcNow - SkillActivated).TotalSeconds; } }

    public void Activate()
    {
        SkillActivated = DateTime.UtcNow;
    }
}



public class SkillsState
{
    Dictionary<SkillID, SkillState> skills;

    public SkillsState()
    {
        skills = new Dictionary<SkillID, SkillState>();

        skills.Add(SkillID.GOLD_RUSH, new SkillState(SkillID.GOLD_RUSH));
    }

    public SkillState Get(SkillID skill) => skills[skill];

    public void ActivateSkill(SkillID skill)
    {
        SkillState state = Get(skill);

        state.Activate();
    }

    public Dictionary<BonusType, double> CalcBonuses()
    {
        return new Dictionary<BonusType, double>();
    }
}
