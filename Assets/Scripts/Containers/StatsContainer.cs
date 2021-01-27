using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SimpleJSON;

namespace GreedyMercs
{
    using GreedyMercs.Quests.Data;
    public class StatsContainer
    {
        public int enemyKills;

        public StatsContainer()
        {

        }

        public virtual void Update(JSONNode node)
        {
            enemyKills = node["enemyKills"].AsInt;
        }

        public virtual JSONNode ToJson()
        {
            return JSON.Parse(JsonUtility.ToJson(this));
        }
    }

    public class PlayerQuestData : StatsContainer
    {
        Dictionary<QuestID, bool> questsClaimed;

        public PlayerQuestData(JSONNode node)
        {
            questsClaimed = new Dictionary<QuestID, bool>();

            Update(node);
            
            UpdateQuestsClaimed(node["questsClaimed"]);
        }

        public void UpdateQuestsClaimed(JSONNode node)
        {
            questsClaimed = new Dictionary<QuestID, bool>();

            foreach (string key in node.Keys)
            {
                questsClaimed.Add(((QuestID)int.Parse(key)), node[key].AsBool);
            }
        }

        public override JSONNode ToJson()
        {
            JSONNode node = base.ToJson();

            JSONNode quests = new JSONObject();

            foreach (var entry in questsClaimed)
            {
                quests.Add(((int)entry.Key).ToString(), entry.Value);
            }

            node.Add("questsClaimed", quests);

            return node;
        }

        public bool IsQuestClaimed(QuestID quest)
        {
            return questsClaimed.ContainsKey(quest) && questsClaimed[quest];
        }
        
        public void ClaimQuest(QuestID quest)
        {
            questsClaimed[quest] = true;
        }
    }

    public class PlayerLifetimeStats
    {
        public int maxPrestigeStage;

        public PlayerLifetimeStats(JSONNode node)
        {
            Update(node);
        }

        public virtual void Update(JSONNode node)
        {
            maxPrestigeStage = node["maxPrestigeStage"].AsInt;
        }

        public virtual JSONNode ToJson()
        {
            return JSON.Parse(JsonUtility.ToJson(this));
        }
    }
}
