﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace GM.Quests
{
    public class QuestsContainer : GM.Core.GMClass
    {
        public DateTime NextDailyRefresh { get; set; }

        List<StaticMercQuest> MercQuestsModels;
        List<StaticDailyQuest> DailyQuestsModels;

        List<int> CompletedMercQuests;
        List<int> CompletedDailyQuests;

        public void Set(QuestsDataResponse model)
        {
            NextDailyRefresh = model.NextDailyRefresh;

            MercQuestsModels = model.MercQuests;
            DailyQuestsModels = model.DailyQuests;

            CompletedDailyQuests = model.CompletedDailyQuests;
            CompletedMercQuests = model.CompletedMercQuests;
        }

        public bool IsDailyQuestsValid => NextDailyRefresh > DateTime.UtcNow;
        public TimeSpan TimeUntilQuestsShouldRefresh => NextDailyRefresh - DateTime.UtcNow;
        public bool IsMercQuestCompleted(int questId) => CompletedMercQuests.Contains(questId);
        public bool IsDailyQuestCompleted(int questId) => CompletedDailyQuests.Contains(questId);

        public int NumQuestsReadyToClaim => NumMercQuestsReadyToComplete + NumDailyQuestsReadyToComplete;
        int NumMercQuestsReadyToComplete => MercQuests.Where(x => !x.IsCompleted && x.CurrentProgress >= 1.0f).Count();
        int NumDailyQuestsReadyToComplete => DailyQuests.Where(x => !x.IsCompleted && x.CurrentProgress >= 1.0f).Count();

        public List<AggregatedDailyQuest> DailyQuests
        {
            get
            {
                List<AggregatedDailyQuest> ls = new();

                DailyQuestsModels.ForEach(quest =>
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

                MercQuestsModels.ForEach(quest =>
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
                if (resp.StatusCode == HTTP.HTTPCodes.Success)
                {
                    CompletedMercQuests.Add(quest.ID);

                    App.Mercs.AddNewUnlockedMerc(resp.UnlockedMerc);
                }

                callback.Invoke(resp.StatusCode == HTTP.HTTPCodes.Success);
            });
        }

        public void SendCompleteDailyQuest(IAggregatedQuest quest, Action<bool> callback)
        {
            App.HTTP.CompleteDailyQuest(quest.ID, (resp) =>
            {
                if (resp.StatusCode == HTTP.HTTPCodes.Success)
                {
                    CompletedDailyQuests.Add(quest.ID);
                }

                callback.Invoke(resp.StatusCode == HTTP.HTTPCodes.Success);
            });
        }

        public void FetchQuests(Action<bool> action)
        {
            App.HTTP.FetchQuests(resp =>
            {
                if (resp.StatusCode == HTTP.HTTPCodes.Success)
                {
                    Set(resp);
                };

                action.Invoke(resp.StatusCode == HTTP.HTTPCodes.Success);
            });
        }
    }
}
