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

    public abstract class AbstractAggregatedQuest : GM.Core.GMClass, IAggregatedQuest
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

        public long LongValue;

        public override bool IsCompleted => App.Quests.IsDailyQuestCompleted(ID);
        public override float CurrentProgress
        {
            get
            {
                return ActionType switch
                {
                    QuestActionType.Prestige => App.Stats.ConfirmedDailyStats.TotalPrestiges / (float)LongValue,
                    QuestActionType.EnemiesDefeated => App.Stats.LocalDailyStats.TotalEnemiesDefeated / (float)LongValue,
                    QuestActionType.BossesDefeated => App.Stats.LocalDailyStats.TotalBossesDefeated / (float)LongValue,
                    QuestActionType.Taps => App.Stats.LocalDailyStats.TotalTaps / (float)LongValue
                };
            }
        }

        public override string Title
        {
            get
            {
                return ActionType switch
                {
                    QuestActionType.Prestige => $"Perform <color=orange>{LongValue}</color> Prestiges",
                    QuestActionType.EnemiesDefeated => $"Defeat <color=orange>{LongValue}</color> Enemies",
                    QuestActionType.BossesDefeated => $"Defeat <color=orange>{LongValue}</color> Stage Bosses",
                    QuestActionType.Taps => $"Deal Tap Damage <color=orange>{LongValue}</color> Times"
                };
            }
        }

    }
}
