using GM.Mercs.Data;
using System;
using System.Collections.Generic;

namespace GM.Quests
{
    public class CompleteMercQuestResponse : GM.HTTP.Requests.ServerResponse
    {
        public MercID UnlockedMerc;
    }

    public class CompleteDailyQuestResponse : GM.HTTP.Requests.ServerResponse
    {
        public int DiamondsRewarded;
    }

    public class QuestsDataResponse : GM.HTTP.Requests.ServerResponse
    {
        public DateTime DateTime;

        public Quests Quests;

        public List<int> CompletedMercQuests;
        public List<int> CompletedDailyQuests;
    }
}
