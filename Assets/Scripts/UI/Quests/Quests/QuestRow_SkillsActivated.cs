using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GreedyMercs.Quests.UI
{
    using GreedyMercs.Quests.Data;

    public class QuestRow_SkillsActivated : QuestRow
    {
        protected override string TargetDataKey => "skillsActivated";
        protected override int ProgressValue => GameState.Quests.skillsActivated;

        void Awake()
        {
            quest = QuestID.SKILLS_ACTIVATED;
        }

        protected override string GetQuestTitle()
        {
            QuestData data = StaticData.Quests.GetQuest(quest);

            return string.Format("Activate {0} Skills", data.GetInt(TargetDataKey));
        }
    }
}