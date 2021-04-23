using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GreedyMercs
{
    public class SkillRow : MonoBehaviour
    {
        [SerializeField] protected SkillID SkillID;

        [Header("References")]
        [SerializeField] Transform SkillButtonsParent;

        [Header("Components")]
        [SerializeField] Button buyButton;

        [Header("Text")]
        [SerializeField] Text nameText;
        [SerializeField] Text energyCostText;
        [SerializeField] Text descriptionText;
        [Space]
        [SerializeField] Text buttonTopText;
        [SerializeField] Text buttonBtmText;

        [Header("Prefabs")]
        [SerializeField] GameObject SkillButtonObject;

        protected SkillState State { get { return GameState.Skills.Get(SkillID);} }

        void Start()
        {
            UpdateUI();

            if (GameState.Skills.IsUnlocked(SkillID))
            {
                Utils.UI.Instantiate(SkillButtonObject, SkillButtonsParent, Vector3.zero);
            }
        }

        protected virtual void OnEnable()
        {
            UpdateUI();
        }

        protected void UpdateUI()
        {
            descriptionText.text = GetDescription();

            UpdateButtonText();

            energyCostText.text     = "";
            buyButton.interactable  = true;

            if (GameState.Skills.IsUnlocked(SkillID))
            {
                SkillSO scriptable = StaticData.SkillList.Get(SkillID);

                energyCostText.text     = State.LevelData.EnergyCost.ToString() + " Energy";
                nameText.text           = string.Format("(Lvl. {0}) {1}", State.level, scriptable.name);
                buyButton.interactable  = !State.IsMaxLevel;
            }
        }

        protected virtual string GetDescription()
        {
            if (!GameState.Skills.IsUnlocked(SkillID))
                return "Locked";

            string effect       = Utils.Format.FormatNumber(StatsCache.Skills.SkillBonus(SkillID) * 100);
            string bonusType    = Utils.Generic.BonusToString(StaticData.SkillList.Get(SkillID).bonusType);

            double duration     = StatsCache.Skills.SkillDuration(SkillID);

            return string.Format("Duration <color=orange>{0}s</color>\nMultiply <color=orange>{1}</color> by <color=orange>{2}%</color>", duration, bonusType, effect);
        }

        void UpdateButtonText()
        {
            if (GameState.Skills.IsUnlocked(SkillID))
            {
                buttonTopText.text = "MAX LEVEL";
                buttonBtmText.text = "-";

                if (!State.IsMaxLevel)
                {
                    SkillLevel nextLevel = GameState.Skills.GetSkillLevel(SkillID, State.level + 1);

                    buttonTopText.text  = string.Format("Level {0} -> {1}", State.level, State.level + 1);
                    buttonBtmText.text  = Utils.Format.FormatNumber(nextLevel.UpgradeCost);
                }
            }

            else
            {
                SkillLevel skillLevel = GameState.Skills.GetSkillLevel(SkillID, 1);

                buttonTopText.text = "LOCKED";
                buttonBtmText.text = Utils.Format.FormatNumber(skillLevel.UpgradeCost);
            }
        }

        public void OnClick()
        {
            SkillLevel skillLevel;

            SkillSO scriptable = StaticData.SkillList.Get(SkillID);

            if (GameState.Skills.IsUnlocked(SkillID))
                skillLevel = GameState.Skills.GetSkillLevel(SkillID, GameState.Skills.Get(SkillID).level + 1);

            else
                skillLevel = GameState.Skills.GetSkillLevel(SkillID, 1);


            if (GameState.Player.gold >= skillLevel.UpgradeCost)
            {
                if (!GameState.Skills.IsUnlocked(SkillID))
                    Utils.UI.Instantiate(SkillButtonObject, SkillButtonsParent, Vector3.zero);

                GameState.Skills.UpgradeSkill(SkillID);

                GameState.Player.gold -= skillLevel.UpgradeCost;

                GameState.Player.currentEnergy = Math.Min(StatsCache.PlayerMaxEnergy(), GameState.Player.currentEnergy + scriptable.EnergyGainedOnUnlock);
            }

            UpdateUI();
        }
    }
}