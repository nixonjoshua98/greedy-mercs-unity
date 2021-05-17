using System;

using UnityEngine;
using UnityEngine.UI;

using Coffee.UIEffects;

namespace GreedyMercs.Quests.UI
{
    using GreedyMercs.Quests.Data;
    using SimpleJSON;

    public class QuestRow : MonoBehaviour
    {
        [SerializeField] QuestID quest;

        [Header("Properties")]
        [SerializeField] string titleTemplate;

        [Header("Effects")]
        [SerializeField] UIShiny shinyEffect;
        
        [Header("Components")]
        [SerializeField] Text questTitle;
        [SerializeField] Text progressText;
        [SerializeField] Image collectImage;
        [Space]
        [SerializeField] Button collectButton;

        [Header("Sprites")]
        [SerializeField] Sprite claimedQuestSprite;

        int ProgressValue
        {
            get
            {
                switch (quest)
                {
                    case QuestID.ENEMY_KILLS:       return GameState.Quests.enemyKills;
                    case QuestID.BOSS_KILLS:        return GameState.Quests.bossKills;
                    case QuestID.PLAYER_CLICKS:     return GameState.Quests.playerClicks;
                    case QuestID.PRESTIGE:          return GameState.Quests.prestiges;
                    case QuestID.SKILLS_ACTIVATED:  return GameState.Quests.skillsActivated;

                    default: return int.MaxValue;
                }
            }
        }

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

            return ProgressValue >= data.targetValue;
        }

        void UpdateUI()
        {
            QuestData data = StaticData.Quests.GetQuest(quest);

            questTitle.text = titleTemplate.Replace("{value}", data.targetValue.ToString());

            progressText.text = GetProgressString();
            
            collectButton.interactable = GetCompleted();

            UpdateShinyEffect();

            if (GameState.Quests.IsQuestClaimed(quest))
            {
                // Change the sprite to show the user they have claimed it
                collectImage.sprite = claimedQuestSprite;

                collectButton.enabled = false;

                CancelInvoke("UpdateUI");  // Stop updating the UI
            }
        }

        void UpdateShinyEffect()
        {
            if (!GameState.Quests.IsQuestClaimed(quest) && GetCompleted())
                shinyEffect.Play(reset: false);
            else
                shinyEffect.Stop(reset: true);
        }

        string GetProgressString()
        {
            QuestData data = StaticData.Quests.GetQuest(quest);

            if (GameState.Quests.IsQuestClaimed(quest))
                return string.Format("{0}/{0}", data.targetValue);

            return string.Format("{0}/{1}", Math.Min(data.targetValue, ProgressValue), data.targetValue);
        }

        // === Callbacks === 

        public void CompleteQuest()
        {
            if (!currentlyClaimingQuest && GetCompleted() && !GameState.Quests.IsQuestClaimed(quest))
            {
                currentlyClaimingQuest = true;

                JSONNode node = Utils.Json.GetDeviceInfo();

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

                UpdateUI();
            }
        }

        void ProcessReward(JSONNode node)
        {
            GameState.Inventory.gems += node["gemReward"].AsInt;
        }
    }
}