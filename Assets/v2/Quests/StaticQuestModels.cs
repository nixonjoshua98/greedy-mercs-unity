using GM.Common.Enums;
using System.Collections.Generic;

namespace GM.Quests
{
    public enum QuestActionType
    {
        Prestige = 0
    }

    public class StaticMercQuest
    {
        public int QuestID;
        public int RequiredStage;
        public MercID RewardMercID;
    }

    public class StaticDailyQuest
    {
        public int QuestID;
        public QuestActionType ActionType;
        public int DiamondsRewarded;

        // Optionals
        public int NumPrestiges; // QuestActionType.Prestige
    }

    public class StaticQuestsModel
    {
        public List<StaticMercQuest> MercQuests;
        public List<StaticDailyQuest> DailyQuests;
    }
}
