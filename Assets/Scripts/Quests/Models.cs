using GM.Common.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace GM.Quests
{
    public enum QuestActionType
    {
        Prestige = 0,
        EnemiesDefeated = 1,
        BossesDefeated = 2,
        Taps = 3,
        StageReached = 4
    }

    public enum QuestType
    {
        MercQuest = 0,
        DailyQuest = 1
    }

    public abstract class AbstractQuest
    {
        [JsonProperty(PropertyName = "QuestID")]
        public int ID;

        public QuestActionType ActionType;
        public long LongValue;
    }

    public class MercQuest : AbstractQuest
    {
        public MercID RewardMercID;
    }

    public class DailyQuest : AbstractQuest
    {
        public int DiamondsRewarded;
    }

    public class Quests
    {
        public List<MercQuest> MercQuests;
        public List<DailyQuest> DailyQuests;
    }
}
