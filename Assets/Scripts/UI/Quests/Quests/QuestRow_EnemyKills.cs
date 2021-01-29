using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GreedyMercs.Quests.UI
{
    using GreedyMercs.Quests.Data;

    public class QuestRow_EnemyKills : QuestRow
    {
        protected override string TargetDataKey => "enemyKills";
        protected override int ProgressValue => GameState.Quests.enemyKills;

        void Awake()
        {
            quest = QuestID.ENEMY_KILLS;
        }

        protected override string GetQuestTitle()
        {
            QuestData data = StaticData.Quests.GetQuest(quest);

            return string.Format("Slay {0} Enemies", data.GetInt(TargetDataKey));
        }
    }
}