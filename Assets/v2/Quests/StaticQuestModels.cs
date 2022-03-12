using GM.Common.Enums;
using System.Collections.Generic;

namespace GM.Quests
{
    public class StaticMercQuest
    {
        public int QuestID;
        public int RequiredStage;
        public MercID RewardMercID;
    }

    public class StaticQuestsModel
    {
        public List<StaticMercQuest> MercQuests;
    }
}
