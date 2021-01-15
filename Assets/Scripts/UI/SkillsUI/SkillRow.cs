using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace Skills.UI
{
    using SkillData;

    public abstract class SkillRow : MonoBehaviour
    {
        [SerializeField] protected SkillID SkillID;

        [Header("References")]
        [SerializeField] Transform SkillButtonsParent;

        [Header("Components")]
        [SerializeField] Button buyButton;

        [Header("Text")]
        [SerializeField] Text energyCostText;
        [SerializeField] Text buyButtonText;
        [SerializeField] Text descriptionText;

        [Header("Prefabs")]
        [SerializeField] GameObject SkillButtonObject;

        protected abstract string SkillDescription();

        void Awake()
        {
            UpdateUI();

            if (GameState.Skills.IsUnlocked(SkillID))
            {
                Utils.UI.Instantiate(SkillButtonObject, SkillButtonsParent, Vector3.zero);
            }
        }

        void OnEnable()
        {
            UpdateUI();
        }

        public void OnClick()
        {
            SkillLevel skillLevel;

            SkillSO scriptable = StaticData.Skills.Get(SkillID);

            if (GameState.Skills.IsUnlocked(SkillID))
                skillLevel = GameState.Skills.GetSkillLevel(SkillID, GameState.Skills.Get(SkillID).level + 1);

            else
                skillLevel = GameState.Skills.GetSkillLevel(SkillID, 1);


            if (GameState.Player.gold >= skillLevel.UpgradeCost)
            {
                if (!GameState.Skills.IsUnlocked(SkillID))
                {
                    Utils.UI.Instantiate(SkillButtonObject, SkillButtonsParent, Vector3.zero);
                }

                GameState.Player.gold -= skillLevel.UpgradeCost;

                GameState.Player.currentEnergy = Math.Min(StatsCache.PlayerMaxEnergy(), GameState.Player.currentEnergy + scriptable.EnergyGainedOnUnlock);

                GameState.Skills.UpgradeSkill(SkillID);
            }

            UpdateUI();
        }

        void UpdateUI()
        {
            descriptionText.text = SkillDescription();

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