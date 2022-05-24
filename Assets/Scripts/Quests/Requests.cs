using GM.Common.Enums;
using GM.HTTP;
using System;
using System.Collections.Generic;

namespace GM.Quests
{
    public class CompleteMercQuestResponse : ServerResponse
    {
        public MercID UnlockedMerc;
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
