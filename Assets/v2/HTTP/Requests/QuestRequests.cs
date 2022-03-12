using GM.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM.HTTP.Requests
{
    public class CompleteQuestRequest : IServerRequest
    {
        public QuestType QuestType;
        public int QuestID;
    }

    public class CompleteMercQuestResponse : ServerResponse
    {
        public MercID UnlockedMerc;
    }
}
