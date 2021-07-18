using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GM
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

        protected SkillState State { get { return SkillsManager.Instance.Get(SkillID);} }

        void Start()
        {
            UpdateUI();

            if (SkillsManager.Instance.IsUnlocked(SkillID))
            {
                CanvasUtils.Instantiate(SkillButtonObject, SkillButtonsParent);
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

            if (SkillsManager.Instance.IsUnlocked(SkillID))
            {
                SkillSO scriptable = StaticData.SkillList.Get(SkillID);

                energyCostText.text     = State.LevelData.EnergyCost.ToString() + " Energy";
                nameText.text           = string.Format("(Lvl. {0}) {1}", State.level, scriptable.name);
                buyButton.interactable  = !State.IsMaxLevel;
            }
        }

        protected virtual string GetDescription()
        {
            if (!SkillsManager.Instance.IsUnlocked(SkillID))
                return "Locked";

            string effect       = FormatString.Number(StatsCache.Skills.SkillBonus(SkillID) * 100);
            string bonusType    = Funcs.BonusString(StaticData.SkillList.Get(SkillID).bonusType);

            double duration     = StatsCache.Skills.SkillDuration(SkillID);

            return string.Format("Duration <color=orange>{0}s</color>\nMultiply <color=orange>{1}</color> by <color=orange>{2}%</color>", duration, bonusType, effect);
        }

        void UpdateButtonText()
        {
            if (SkillsManager.Instance.IsUnlocked(SkillID))
            {
                buttonTopText.text = "MAX";
                buttonBtmText.text = "-";

                if (!State.IsMaxLevel)
                {
                    SkillLevel nextLevel = SkillsManager.Instance.GetSkillLevel(SkillID, State.level + 1);

                    buttonTopText.text  = string.Format("Level {0} -> {1}", State.level, State.level + 1);
                    buttonBtmText.text  = FormatString.Number(nextLevel.UpgradeCost);
                }
            }

            else
            {
                SkillLevel skillLevel = SkillsManager.Instance.GetSkillLevel(SkillID, 1);

                buttonTopText.text = "LOCKED";
                buttonBtmText.text = FormatString.Number(skillLevel.UpgradeCost);
            }
        }

        public void OnClick()
        {
            SkillLevel skillLevel;

            SkillSO scriptable = StaticData.SkillList.Get(SkillID);

            if (SkillsManager.Instance.IsUnlocked(SkillID))
                skillLevel = SkillsManager.Instance.GetSkillLevel(SkillID, SkillsManager.Instance.Get(SkillID).level + 1);

            else
                skillLevel = SkillsManager.Instance.GetSkillLevel(SkillID, 1);


            if (GameState.Player.gold >= skillLevel.UpgradeCost)
            {
                if (!SkillsManager.Instance.IsUnlocked(SkillID))
                    CanvasUtils.Instantiate(SkillButtonObject, SkillButtonsParent);

                SkillsManager.Instance.UpgradeSkill(SkillID);

                GameState.Player.gold -= skillLevel.UpgradeCost;

                GameState.Player.currentEnergy += scriptable.EnergyGainedOnUnlock;
            }

            UpdateUI();
        }
    }
}