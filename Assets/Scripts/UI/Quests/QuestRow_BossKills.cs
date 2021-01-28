using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GreedyMercs.Quests.UI
{
    using GreedyMercs.Quests.Data;

    public class QuestRow_BossKills : QuestRow
    {
        protected override string TargetDataKey => "bossKills";
        protected override int ProgressValue => GameState.Quests.bossKills;

        void Awake()
        {
            quest = QuestID.BOSS_KILLS;
        }

        protected override string GetQuestTitle()
        {
            QuestData data = StaticData.Quests.GetQuest(quest);

            return string.Format("Defeat {0} Stage Bosses", data.GetInt(TargetDataKey));
        }
    }
}