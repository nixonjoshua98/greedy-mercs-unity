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
        [SerializeField] Text buyButtonText;
        [SerializeField] Text energyCostText;
        [SerializeField] Text descriptionText;

        [Header("Prefabs")]
        [SerializeField] GameObject SkillButtonObject;

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

        protected virtual string GetDescription()
        {
            if (!GameState.Skills.IsUnlocked(SkillID))
                return "Locked";

            string effect = Utils.Format.FormatNumber(StatsCache.Skills.SkillBonus(SkillID) * 100);
            double duration = StatsCache.Skills.SkillDuration(SkillID);

            string bonusType = Utils.Generic.BonusToString(StaticData.SkillList.Get(SkillID).bonusType);

            return string.Format("Multiply <color=orange>{1}</color> by <color=orange>{2}%</color> for <color=orange>{0}s</color>", duration, bonusType, effect);
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

        protected void UpdateUI()
        {
            descriptionText.text = GetDescription();

            if (GameState.Skills.IsUnlocked(SkillID))
            {
                SkillState state        = GameState.Skills.Get(SkillID);
                SkillLevel skillLevel   = state.LevelData;

                energyCostText.text     = skillLevel.EnergyCost.ToString() + " Energy";

                if (state.IsMaxLevel)
                    buyButtonText.text = "MAX";

                else
                {
                    SkillLevel nextLevel    = GameState.Skills.GetSkillLevel(SkillID, state.level + 1);
                    buyButtonText.text      = Utils.Format.FormatNumber(nextLevel.UpgradeCost);
                }

                SkillSO scriptable = StaticData.SkillList.Get(SkillID);

                nameText.text = string.Format("(Lvl. {0}) {1}", state.level, scriptable.name);

                buyButton.interactable  = !state.IsMaxLevel;
            }

            else
            {
                SkillLevel skillLevel = GameState.Skills.GetSkillLevel(SkillID, 1);

                buyButtonText.text      = string.Format("Unlock\n{0}", Utils.Format.FormatNumber(skillLevel.UpgradeCost));
                energyCostText.text     = "";
                buyButton.interactable  = true;
            }
        }
    }
}