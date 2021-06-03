using System;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using SimpleJSON;

namespace GM
{
    using GM;

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



    public class SkillsManager : IBonusManager
    {
        public static SkillsManager Instance = null;

        Dictionary<SkillID, SkillState> skills;

        public static void Create(JSONNode node)
        {
            Instance = new SkillsManager(node);
        }

        SkillsManager(JSONNode node)
        {
            skills = new Dictionary<SkillID, SkillState>();

            foreach (string key in node.Keys)
            {
                SkillID skill = (SkillID)int.Parse(key);

                SkillState restoredState = JsonUtility.FromJson<SkillState>(node[key].ToString());

                skills.Add(skill, new SkillState(skill) { level = restoredState.level });
            }
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

        // = = = Special methods = = = //
        public List<KeyValuePair<BonusType, double>> Bonuses()
        {
            List<KeyValuePair<BonusType, double>> ls = new List<KeyValuePair<BonusType, double>>();

            foreach (var entry in skills)
            {
                if (entry.Value.IsActive)
                {
                    SkillSO data = StaticData.SkillList.Get(entry.Key);

                    ls.Add(new KeyValuePair<BonusType, double>(data.bonusType, entry.Value.LevelData.BonusValue));
                }
            }

            return ls;
        }
    }
}