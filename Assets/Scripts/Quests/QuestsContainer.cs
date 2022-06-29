using System;
using System.Collections.Generic;
using System.Linq;

namespace GM.Quests
{
    public class QuestsContainer : GM.Core.GMClass
    {
        public DateTime QuestsCreatedAt { get; set; }

        private List<MercQuest> MercQuestsModels;
        private List<DailyQuest> DailyQuestsModels;
        private List<int> CompletedMercQuests;
        private List<int> CompletedDailyQuests;

        public void Set(QuestsDataResponse model)
        {
            QuestsCreatedAt = model.DateTime;

            MercQuestsModels = model.Quests.MercQuests;
            DailyQuestsModels = model.Quests.DailyQuests;

            CompletedDailyQuests = model.CompletedDailyQuests;
            CompletedMercQuests = model.CompletedMercQuests;
        }

        public bool IsDailyQuestsValid => App.DailyRefresh.Current.IsBetween(QuestsCreatedAt);

        public bool IsQuestCompleted(QuestType questType, int questId)
        {
            return questType switch
            {
                QuestType.MercQuest => CompletedMercQuests.Contains(questId),
                QuestType.DailyQuest => CompletedDailyQuests.Contains(questId),
                _ => throw new NotImplementedException()
            };
        }

        public int NumQuestsReadyToClaim => NumMercQuestsReadyToComplete + NumDailyQuestsReadyToComplete;

        private int NumMercQuestsReadyToComplete => MercQuests.Where(x => !x.IsCompleted && x.CurrentProgress >= 1.0f).Count();

        private int NumDailyQuestsReadyToComplete => DailyQuests.Where(x => !x.IsCompleted && x.CurrentProgress >= 1.0f).Count();

        public List<AggregatedDailyQuest> DailyQuests
        {
            get
            {
                List<AggregatedDailyQuest> ls = new();

                DailyQuestsModels.ForEach(quest =>
                {
                    ls.Add(new()
                    {
                        ID = quest.ID,
                        ActionType = quest.ActionType,
                        DiamondsRewarded = quest.DiamondsRewarded,
                        LongValue = quest.LongValue
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
                        ID = quest.ID,
                        ActionType = quest.ActionType,
                        LongValue = quest.LongValue
                    });
                });

                return ls;
            }
        }

        public void SendCompleteMercQuest(AbstractAggregatedQuest quest, Action<bool> callback)
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

        public void SendCompleteDailyQuest(AbstractAggregatedQuest quest, Action<bool> callback)
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
