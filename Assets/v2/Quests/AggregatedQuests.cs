using GM.Common.Enums;
using System;

namespace GM.Quests
{
    public interface IAggregatedQuest
    {
        public string Title { get; }
        public int ID { get; }
        public bool IsCompleted { get; }
        public float CurrentProgress { get; }
    }


    public abstract class AbstractAggregatedQuest: GM.Core.GMClass, IAggregatedQuest
    {
        public int ID { get; set; }

        public abstract string Title { get; }
        public abstract bool IsCompleted { get; }
        public abstract float CurrentProgress { get; }

    }


    public class AggregatedMercQuest : AbstractAggregatedQuest
    {
        public int RequiredStage;
        public MercID RewardMercID;

        public override bool IsCompleted => App.Quests.IsMercQuestCompleted(ID);
        public override float CurrentProgress => Math.Min(1.0f, App.GameState.Stage / Math.Max(1, (float)RequiredStage));
        public override string Title => $"Reach Stage <color=orange>{RequiredStage}</color>";
    }


    public class AggregatedDailyQuest : AbstractAggregatedQuest
    {
        public QuestActionType ActionType;
        public int DiamondsRewarded;

        // Optionals
        public int NumPrestiges; // QuestActionType.Prestige


        public override bool IsCompleted => false;
        public override float CurrentProgress => 0;
        public override string Title
        {
            get
            {
                return ActionType switch
                {
                    QuestActionType.Prestige => $"Perform <color=orange>{NumPrestiges}</color> Prestiges",
                };
            }
        }

    }
}
