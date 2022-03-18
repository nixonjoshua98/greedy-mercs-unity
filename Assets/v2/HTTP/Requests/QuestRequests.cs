using GM.Common.Enums;

namespace GM.HTTP.Requests
{
    public class CompleteQuestRequest : IServerRequest
    {
        public int QuestID;
    }

    public class CompleteMercQuestResponse : ServerResponse
    {
        public MercID UnlockedMerc;
    }
}
