using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GreedyMercs.Quests.UI
{
    using GreedyMercs.Quests.Data;
    using SimpleJSON;

    public abstract class QuestRow : MonoBehaviour
    {
        protected QuestID quest;

        [SerializeField] Text progressText;
        [SerializeField] Text questTitle;

        [SerializeField] Image collectImage;
        [SerializeField] Button collectButton;

        [Header("Sprites")]
        [SerializeField] Sprite claimedQuestSprite;

        //
        bool currentlyClaimingQuest = false;

        void Awake()
        {
            UpdateUI();
        }

        void OnEnable() => InvokeRepeating("UpdateUI", 0.0f, 0.5f);
        void OnDisable() => CancelInvoke("UpdateUI");

        protected abstract bool GetCompleted();
        protected abstract string GetQuestTitle();
        protected abstract string GetProgressString();

        void UpdateUI()
        {
            questTitle.text = GetQuestTitle();
            progressText.text = GetProgressString();

            collectButton.interactable = GetCompleted();

            if (GameState.Quests.IsQuestClaimed(quest))
            {
                collectImage.sprite = claimedQuestSprite;

                Destroy(collectButton);

                CancelInvoke("UpdateUI");
            }
        }

        public void OnClick()
        {
            if (!currentlyClaimingQuest && GetCompleted() && !GameState.Quests.IsQuestClaimed(quest))
            {
                currentlyClaimingQuest = true;

                JSONNode node = Utils.Json.GetDeviceNode();

                node["questId"] = (int)quest;

                Server.ClaimQuestReward(this, OnServerCallback, node);
            }
        }

        void OnServerCallback(long code, string compressed)
        {
            if (code == 200)
            {
                currentlyClaimingQuest = false;

                GameState.Quests.ClaimQuest(quest);

                ProcessReward(Utils.Json.Decompress(compressed));
            }
        }

        void ProcessReward(JSONNode node)
        {
            GameState.Player.gems += node["gemReward"].AsInt;
        }

    }
}