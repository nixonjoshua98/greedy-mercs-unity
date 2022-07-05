using GM.Common.Enums;
using GM.UserStats;
using System;

namespace GM.Quests
{
    public abstract class AbstractAggregatedQuest : GM.Core.GMClass
    {
        public int ID;
        public long LongValue;

        public QuestActionType ActionType;
        public readonly QuestType QuestType;

        public AbstractAggregatedQuest(QuestType questType)
        {
            QuestType = questType;
        }

        public bool IsCompleted => App.Quests.IsQuestCompleted(QuestType, ID);

        public float CurrentProgress
        {
            get
            {
                UserAccountStats stats = QuestType switch
                {
                    QuestType.MercQuest => App.Stats.ConfirmedLifetimeStats,
                    QuestType.DailyQuest => App.Stats.LocalDailyStats,
                    _ => throw new Exception()
                };

                return ActionType switch
                {
                    QuestActionType.Prestige => stats.TotalPrestiges / (float)LongValue,
                    QuestActionType.EnemiesDefeated => stats.TotalEnemiesDefeated / (float)LongValue,
                    QuestActionType.BossesDefeated => stats.TotalBossesDefeated / (float)LongValue,
                    QuestActionType.Taps => stats.TotalTaps / (float)LongValue,
                    QuestActionType.StageReached => App.GameState.Stage / (float)LongValue, // Refers to current stage (not prestige stage)
                    _ => throw new Exception(nameof(CurrentProgress))
                };
            }
        }

        public string Title => ActionType switch
        {
            QuestActionType.Prestige => $"Perform <color=orange>{LongValue}</color> Prestiges",
            QuestActionType.EnemiesDefeated => $"Defeat <color=orange>{LongValue}</color> Enemies",
            QuestActionType.BossesDefeated => $"Defeat <color=orange>{LongValue}</color> Stage Bosses",
            QuestActionType.Taps => $"Deal Tap Damage <color=orange>{LongValue}</color> Times",
            QuestActionType.StageReached => $"Reach stage <color=orange>{LongValue}</color>",
            _ => throw new System.Exception()
        };
    }

    public class AggregatedMercQuest : AbstractAggregatedQuest
    {
        public MercID RewardMercID;

        public AggregatedMercQuest(MercQuest quest) : base(QuestType.MercQuest)
        {
            RewardMercID = quest.RewardMercID;
        }
    }

    public class AggregatedDailyQuest : AbstractAggregatedQuest
    {
        public int DiamondsRewarded;

        public AggregatedDailyQuest() : base(QuestType.DailyQuest)
        {

        }
    }
}
