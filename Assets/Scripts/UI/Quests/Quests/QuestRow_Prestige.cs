using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GreedyMercs.Quests.UI
{
    using GreedyMercs.Quests.Data;

    public class QuestRow_Prestige : QuestRow
    {
        protected override string TargetDataKey => "prestiges";
        protected override int ProgressValue => GameState.Quests.prestiges;

        void Awake()
        {
            quest = QuestID.PRESTIGE;
        }

        protected override string GetQuestTitle()
        {
            QuestData data = StaticData.Quests.GetQuest(quest);

            return string.Format("Cash out {0} Times", data.GetInt(TargetDataKey));
        }
    }
}