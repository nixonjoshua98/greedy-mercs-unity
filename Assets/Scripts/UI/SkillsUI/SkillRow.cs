using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace Skills.UI
{
    public class SkillRow : MonoBehaviour
    {
        [SerializeField] SkillData.SkillID SkillID;

        [Header("References")]
        [SerializeField] Transform SkillButtonsParent;

        [Header("Components")]
        [SerializeField] Button buyButton;
        [SerializeField] Text buyButtonText;

        [Header("Prefabs")]
        [SerializeField] GameObject SkillButtonObject;

        void Awake()
        {
            UpdateUI();

            if (GameState.Skills.IsUnlocked(SkillID))
            {
                Utils.UI.Instantiate(SkillButtonObject, SkillButtonsParent, Vector3.zero);
            }
        }

        public void OnClick()
        {
            if (!GameState.Skills.IsUnlocked(SkillID))
            {
                var data = StaticData.Skills.Get(SkillID);

                if (GameState.Player.gold >= data.UnlockCost)
                {
                    GameState.Player.gold -= data.UnlockCost;

                    GameState.Player.currentEnergy = Mathf.Min(StatsCache.PlayerMaxEnergy(), GameState.Player.currentEnergy + data.EnergyGainedOnUnlock);

                    GameState.Skills.UnlockSkill(SkillID);

                    Utils.UI.Instantiate(SkillButtonObject, SkillButtonsParent, Vector3.zero);
                }
            }

            UpdateUI();
        }

        void UpdateUI()
        {
            buyButton.interactable = !GameState.Skills.IsUnlocked(SkillID);

            if (GameState.Skills.IsUnlocked(SkillID))
            {
                buyButtonText.text = "UNLOCKED";
            }

            else
            {
                buyButtonText.text = string.Format("Unlock\n{0}", Utils.Format.FormatNumber(StaticData.Skills.Get(SkillID).UnlockCost));
            }
        }
    }
}
