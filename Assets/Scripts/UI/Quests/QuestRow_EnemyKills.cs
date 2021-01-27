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
        void Start()
        {
            quest = QuestID.ENEMY_KILLS;
        }

        protected override bool GetCompleted()
        {
            if (GameState.Quests.IsQuestClaimed(quest))
                return true;

            QuestData data = StaticData.Quests.GetQuest(quest);

            return Math.Min(data.GetInt("enemyKills"), GameState.Quests.enemyKills) >= data.GetInt("enemyKills");
        }

        protected override string GetProgressString()
        {
            QuestData data = StaticData.Quests.GetQuest(quest);

            if (GameState.Quests.IsQuestClaimed(quest))
                return string.Format("{0}/{1}", data.GetInt("enemyKills"), data.GetInt("enemyKills"));

            int target      = data.GetInt("enemyKills");
            int progress    = Math.Min(data.GetInt("enemyKills"), GameState.Quests.enemyKills);

            return string.Format("{0}/{1}", progress, target);
        }

        protected override string GetQuestTitle()
        {
            QuestData data = StaticData.Quests.GetQuest(quest);

            return string.Format("Slay {0} Enemies", data.GetInt("enemyKills"));
        }
    }
}