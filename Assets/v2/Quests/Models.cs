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

    public class StaticQuestsModel
    {
        public List<StaticMercQuest> MercQuests;
        public List<StaticDailyQuest> DailyQuests;
    }

    public class GetQuestsResponse : GM.HTTP.ServerResponse
    {

    }

    public class UserQuestsModel
    {
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime NextDailyQuestsRefresh { get; set; }

        public List<int> CompletedMercQuests;
        public List<int> CompletedDailyQuests;
    }
}
