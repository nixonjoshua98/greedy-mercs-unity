﻿using System;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SimpleJSON;

namespace GreedyMercs
{
    using GreedyMercs.Quests.Data;
    public class StatsContainer
    {
        public int enemyKills = 0;

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

        public DateTime lastReset;

        public bool IsValid { get { return lastReset >= GameState.LastDailyReset; } }

        public PlayerQuestData(JSONNode node)
        {
            questsClaimed = new Dictionary<QuestID, bool>();

            lastReset = node.HasKey("lastReset") ? node["lastReset"].AsLong.ToUnixDatetime() : GameState.LastDailyReset;

            if (IsValid)
                Update(node);
            else
                Reset();
            
            UpdateQuestsClaimed(node["questsClaimed"]);
        }

        public void Reset()
        {
            if (IsValid)
                return;

            enemyKills = 0;

            lastReset = GameState.LastDailyReset;

            questsClaimed.Clear();
        }


        // Update which quests have been completed (called with local and server data)
        public void UpdateQuestsClaimed(JSONNode node)
        {
            questsClaimed = new Dictionary<QuestID, bool>();

            foreach (string key in node.Keys)
            {
                questsClaimed.Add(((QuestID)int.Parse(key)), node[key].AsBool);
            }
        }

        public bool IsQuestClaimed(QuestID quest) => questsClaimed.ContainsKey(quest) && questsClaimed[quest];
        public void ClaimQuest(QuestID quest) => questsClaimed[quest] = true;

        public override JSONNode ToJson()
        {
            JSONNode node = base.ToJson();

            JSONNode quests = new JSONObject();

            foreach (var entry in questsClaimed)
                quests.Add(((int)entry.Key).ToString(), entry.Value);

            node.Add("questsClaimed", quests);
            node.Add("lastReset", lastReset.ToUnixMilliseconds());

            return node;
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
