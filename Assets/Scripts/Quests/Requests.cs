using SRC.Mercs.Data;
using System;
using System.Collections.Generic;

namespace SRC.Quests
{
    public class CompleteMercQuestResponse : SRC.HTTP.Requests.ServerResponse
    {
        public MercID UnlockedMerc;
    }

    public class CompleteDailyQuestResponse : SRC.HTTP.Requests.ServerResponse
    {
        public int DiamondsRewarded;
    }

    public class QuestsDataResponse : SRC.HTTP.Requests.ServerResponse
    {
        public DateTime DateTime;

        public Quests Quests;

        public List<int> CompletedMercQuests;
        public List<int> CompletedDailyQuests;
    }
}
