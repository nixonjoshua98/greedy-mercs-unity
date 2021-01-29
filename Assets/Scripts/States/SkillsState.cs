using System;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using SimpleJSON;

namespace GreedyMercs
{
    public class SkillState
    {
        SkillID SkillID;

        public int level;

        public DateTime skillActivated;

        public SkillLevel LevelData { get { return StaticData.SkillList.Get(SkillID).GetLevel(level); } }

        public bool IsMaxLevel { get { return StaticData.SkillList.Get(SkillID).MaxLevel == level; } }

        public SkillState(SkillID skill)
        {
            level = 1;

            SkillID = skill;
        }

        public bool IsActive { get { return TimeSinceActivated <= StatsCache.Skills.SkillDuration(SkillID); } }

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

                SkillState restoredState = JsonUtility.FromJson<SkillState>(node[key].ToString());

                skills.Add(skill, new SkillState(skill) { level = restoredState.level });
            }
        }

        public void Clear()
        {
            skills = new Dictionary<SkillID, SkillState>();
        }

        public JSONNode ToJson()
        {
            JSONNode node = new JSONObject();

            foreach (var entry in skills)
            {
                node.Add(((int)entry.Key).ToString(), JSON.Parse(JsonUtility.ToJson(entry.Value)));
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
                    SkillSO data = StaticData.SkillList.Get(entry.Key);

                    bonuses[data.bonusType] = bonuses.GetOrVal(data.bonusType, 1) * entry.Value.LevelData.BonusValue;
                }
            }

            return bonuses;
        }


        public void UpgradeSkill(SkillID skill)
        {
            if (skills.ContainsKey(skill))
                skills[skill].level++;

            else
                skills.Add(skill, new SkillState(skill));
        }

        public bool IsUnlocked(SkillID skill) => skills.ContainsKey(skill);

        public SkillLevel GetSkillLevel(SkillID skill, int level) => StaticData.SkillList.Get(skill).GetLevel(level);

        public SkillState Get(SkillID skill) => skills[skill];

        public void ActivateSkill(SkillID skill)
        {
            SkillState state = Get(skill);

            state.Activate();
        }

        public List<SkillSO> Unlocked()
        {
            List<SkillSO> unlocked = new List<SkillSO>();

            foreach (SkillSO skill in StaticData.SkillList.SkillList)
            {
                if (IsUnlocked(skill.SkillID))
                {
                    unlocked.Add(skill);
                }
            }

            return unlocked;
        }
    }
}