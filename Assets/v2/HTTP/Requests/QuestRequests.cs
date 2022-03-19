using GM.Common.Enums;

namespace GM.HTTP.Requests
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
    }

    public class CompleteDailyQuestResponse : ServerResponse
    {
        public int DiamondsRewarded;
    }
}
