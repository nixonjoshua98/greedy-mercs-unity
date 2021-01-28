using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GreedyMercs.Quests.UI
{
    using GreedyMercs.Quests.Data;

    public class QuestRow_PlayerClicks : QuestRow
    {
        protected override string TargetDataKey => "playerClicks";
        protected override int ProgressValue => GameState.Quests.playerClicks;

        void Awake()
        {
            quest = QuestID.PLAYER_CLICKS;
        }

        protected override string GetQuestTitle()
        {
            QuestData data = StaticData.Quests.GetQuest(quest);

            return string.Format("Click {0} Times", data.GetInt(TargetDataKey));
        }
    }
}