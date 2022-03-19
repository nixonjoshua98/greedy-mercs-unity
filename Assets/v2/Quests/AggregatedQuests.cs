using GM.Common.Enums;
using System;

namespace GM.Quests
{
    public class AggregatedUserMercQuest : GM.Core.GMClass
    {
        public int ID;
        public int RequiredStage;
        public MercID RewardMercID;

        public bool IsCompleted => App.Quests.IsMercQuestCompleted(ID);
        public float CurrentProgress => Math.Min(1.0f, App.GameState.Stage / Math.Max(1, (float)RequiredStage));
        public string Title => $"Reach Stage <color=orange>{RequiredStage}</color>";
    }
}
