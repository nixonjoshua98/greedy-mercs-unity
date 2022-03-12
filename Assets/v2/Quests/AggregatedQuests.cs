using GM.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM.Quests
{
    public class AggregatedUserMercQuest : GM.Core.GMClass
    {
        public QuestType QuestType = QuestType.Merc;

        public int ID;
        public int RequiredStage;
        public MercID RewardMercID;

        public bool IsCompleted => App.DataContainers.Quests.IsMercQuestCompleted(ID);
        public float CurrentProgress => Math.Min(1.0f, App.DataContainers.GameState.Stage / (float)RequiredStage);



        public string Title => $"Reach Stage <color=orange>{RequiredStage}</color>";
    }
}
