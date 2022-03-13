﻿using GM.Common.Enums;
using System;
using System.Linq;
using System.Collections.Generic;

namespace GM.Quests
{
    public class QuestsContainer : GM.Core.GMClass
    {
        StaticQuestsModel StaticQuests;
        UserQuestsModel UserQuests;

        public void Set(StaticQuestsModel staticQuests, UserQuestsModel userQuests)
        {
            StaticQuests = staticQuests;
            UserQuests = userQuests;
        }

        public bool IsMercQuestCompleted(int questId) => UserQuests.CompletedMercQuests.Contains(questId);

        public int NumQuestsReadyToClaim => NumMercQuestsReadyToComplete;

        public int NumMercQuestsReadyToComplete => MercQuests.Where(x => !x.IsCompleted && x.CurrentProgress >= 1.0f).Count();

        void SetQuestComplete(QuestType questType, int questId)
        {
            switch (questType)
            {
                case QuestType.Merc:
                    UserQuests.CompletedMercQuests.Add(questId);
                    break;

                default:
                    throw new Exception();
            }
        }

        public List<AggregatedUserMercQuest> MercQuests
        {
            get
            {
                List<AggregatedUserMercQuest> ls = new List<AggregatedUserMercQuest>();

                StaticQuests.MercQuests.ForEach(quest =>
                {
                    ls.Add(new()
                    {
                        ID = quest.QuestID,
                        RequiredStage = quest.RequiredStage,
                        RewardMercID = quest.RewardMercID
                    });
                });

                return ls;
            }
        }

        public void SendCompleteMercQuest(AggregatedUserMercQuest quest, Action<bool> callback)
        {
            App.HTTP.CompleteMercRequest(quest.ID, (resp) =>
            {
                if (resp.StatusCode == 200)
                {
                    SetQuestComplete(QuestType.Merc, quest.ID);

                    App.Mercs.AddNewUnlockedMerc(resp.UnlockedMerc);
                }

                callback.Invoke(resp.StatusCode == 200);
            });
        }
    }
}