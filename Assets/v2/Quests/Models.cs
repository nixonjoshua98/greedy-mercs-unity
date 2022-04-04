using GM.Common.Enums;
using System.Collections.Generic;

namespace GM.Quests
{
    public enum QuestActionType
    {
        Prestige = 0,
        EnemiesDefeated = 1,
        BossesDefeated = 2,
        Taps = 3
    }

    public class MercQuest
    {
        public int QuestID;
        public int RequiredStage;
        public MercID RewardMercID;
    }

    public class DailyQuest
    {
        public int QuestID;
        public QuestActionType ActionType;
        public int DiamondsRewarded;

        public long LongValue;
    }

    public class Quests
    {
        public List<MercQuest> MercQuests;
        public List<DailyQuest> DailyQuests;
    }
}
