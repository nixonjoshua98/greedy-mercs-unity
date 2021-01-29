using System;

using UnityEngine;
using UnityEngine.UI;

namespace GreedyMercs.Quests.UI
{
    using GreedyMercs.Quests.Data;
    using SimpleJSON;

    public abstract class QuestRow : MonoBehaviour
    {
        protected QuestID quest;

        [Header("Components")]
        [SerializeField] Text questTitle;
        [SerializeField] Text rewardText;
        [SerializeField] Text progressText;
        [SerializeField] Image collectImage;
        [Space]
        [SerializeField] Button collectButton;

        [Header("Sprites")]
        [SerializeField] Sprite claimedQuestSprite;

        protected abstract string TargetDataKey { get; }
        protected abstract int ProgressValue { get; }

        //
        bool currentlyClaimingQuest = false;

        void Start()
        {
            UpdateUI();
        }

        void OnEnable()
        {
            if (!GameState.Quests.IsQuestClaimed(quest)) 
                InvokeRepeating("UpdateUI", 0.0f, 0.5f);
        }

        void OnDisable() => CancelInvoke("UpdateUI");

        protected bool GetCompleted()
        {
            if (GameState.Quests.IsQuestClaimed(quest))
                return true;

            QuestData data = StaticData.Quests.GetQuest(quest);

            return ProgressValue >= data.GetInt(TargetDataKey);
        }

        protected abstract string GetQuestTitle();

        protected virtual string GetProgressString()
        {
            QuestData data = StaticData.Quests.GetQuest(quest);

            int target = data.GetInt(TargetDataKey);

            if (GameState.Quests.IsQuestClaimed(quest))
                return string.Format("{0}/{0}", target);

            return string.Format("{0}/{1}", Math.Min(target, ProgressValue), target);
        }

        protected virtual string GetRewardString()
        {
            QuestData data = StaticData.Quests.GetQuest(quest);

            return string.Format("{0}x Gems", data.GetInt("gemReward"));
        }

        void UpdateUI()
        {
            questTitle.text     = GetQuestTitle();
            progressText.text   = GetProgressString();

            if (collectButton)  // Check if the button exists since we destroy it at some point
                collectButton.interactable = GetCompleted();

            // Quest has been claimed
            if (GameState.Quests.IsQuestClaimed(quest))
            {
                // Change the sprite to show the user they have claimed it
                collectImage.sprite = claimedQuestSprite;

                Destroy(collectButton);

                CancelInvoke("UpdateUI");  // Stop updating the UI
            }
        }

        public void OnClick()
        {
            if (!currentlyClaimingQuest && GetCompleted() && !GameState.Quests.IsQuestClaimed(quest))
            {
                currentlyClaimingQuest = true;

                JSONNode node = Utils.Json.GetDeviceNode();

                node["questId"] = (int)quest;

                Server.ClaimQuestReward(OnServerCallback, node);
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