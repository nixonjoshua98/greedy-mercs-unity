﻿using System;
using System.Collections.Generic;
using System.Linq;

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

        public TimeSpan TimeUntilQuestsShouldRefresh => (UserQuests.LastQuestsRefresh + TimeSpan.FromDays(1)) - DateTime.UtcNow;
        public bool IsMercQuestCompleted(int questId) => UserQuests.CompletedMercQuests.Contains(questId);
        public bool IsDailyQuestCompleted(int questId) => UserQuests.CompletedDailyQuests.Contains(questId);

        public int NumQuestsReadyToClaim => NumMercQuestsReadyToComplete + NumDailyQuestsReadyToComplete;
        int NumMercQuestsReadyToComplete => MercQuests.Where(x => !x.IsCompleted && x.CurrentProgress >= 1.0f).Count();
        int NumDailyQuestsReadyToComplete => DailyQuests.Where(x => !x.IsCompleted && x.CurrentProgress >= 1.0f).Count();

        public List<AggregatedDailyQuest> DailyQuests
        {
            get
            {
                List<AggregatedDailyQuest> ls = new();

                StaticQuests.DailyQuests.ForEach(quest =>
                {
                    ls.Add(new()
                    {
                        ID = quest.QuestID,
                        ActionType = quest.ActionType,
                        DiamondsRewarded = quest.DiamondsRewarded,
                        NumPrestiges = quest.NumPrestiges
                    });
                });

                return ls;
            }
        }

        public List<AggregatedMercQuest> MercQuests
        {
            get
            {
                List<AggregatedMercQuest> ls = new();

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

        public void SendCompleteMercQuest(IAggregatedQuest quest, Action<bool> callback)
        {
            App.HTTP.CompleteMercQuest(quest.ID, (resp) =>
            {
                if (resp.StatusCode == 200)
                {
                    UserQuests.CompletedMercQuests.Add(quest.ID);

                    App.Mercs.AddNewUnlockedMerc(resp.UnlockedMerc);
                }

                callback.Invoke(resp.StatusCode == 200);
            });
        }

        public void SendCompleteDailyQuest(IAggregatedQuest quest, Action<bool> callback)
        {
            App.HTTP.CompleteDailyQuest(quest.ID, (resp) =>
            {
                if (resp.StatusCode == HTTP.HTTPCodes.Success)
                {
                    UserQuests.CompletedDailyQuests.Add(quest.ID);
                }

                callback.Invoke(resp.StatusCode == HTTP.HTTPCodes.Success);
            });
        }
    }
}
