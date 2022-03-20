using GM.Common.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
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

        public int NumPrestiges; // QuestActionType.Prestige
    }

    public class QuestsDataResponse : GM.HTTP.ServerResponse
    {
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime NextDailyRefresh { get; set; }

        public List<StaticMercQuest> MercQuests;
        public List<StaticDailyQuest> DailyQuests;

        public List<int> CompletedMercQuests;
        public List<int> CompletedDailyQuests;
    }
}
