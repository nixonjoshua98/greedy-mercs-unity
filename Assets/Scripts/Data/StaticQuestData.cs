using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GreedyMercs.Quests.Data
{
    public enum QuestID
    {
        ENEMY_KILLS         = 0,
        BOSS_KILLS          = 1,
        PLAYER_CLICKS       = 2,
        PRESTIGE            = 3,
        SKILLS_ACTIVATED    = 4
    }

    public struct QuestData
    {
        JSONNode node;

        public QuestData(JSONNode _node)
        {
            node = _node;
        }

        public int GetInt(string key) => node[key].AsInt;
    }

    public class StaticQuestData
    {
        Dictionary<QuestID, QuestData> quests;

        public StaticQuestData(JSONNode node)
        {
            Update(node);
        }

        public void Update(JSONNode node)
        {
            quests = new Dictionary<QuestID, QuestData>();

            foreach (string key in node.Keys)
            {
                quests.Add((QuestID)int.Parse(key), new QuestData(node[key]));
            }
        }

        public QuestData GetQuest(QuestID quest) => quests[quest];
    }

}