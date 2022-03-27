using GM.Common.Enums;
using GM.HTTP;
using GM.PlayerStats;
using System;
using System.Collections.Generic;

namespace GM.Quests
{
    public class CompleteMercQuestRequest : IServerRequest
    {
        public int QuestID;
        public int HighestStageReached;
    }

    public class CompleteMercQuestResponse : ServerResponse
    {
        public MercID UnlockedMerc;
    }

    public class CompleteDailyQuestRequest : IServerRequest
    {
        public int QuestID;
        public TimedPlayerStatsModel LocalDailyStats;
    }

    public class CompleteDailyQuestResponse : ServerResponse
    {
        public int DiamondsRewarded;
    }

    public class QuestsDataResponse : GM.HTTP.ServerResponse
    {
        public DateTime DateTime;

        public Quests Quests;

        public List<int> CompletedMercQuests;
        public List<int> CompletedDailyQuests;
    }
}
