using GM.Bounties.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GM.Bounties.UI
{
    public class BountyPopup : GM.UI.PopupPanelBase
    {
        [Header("Text Elements")]
        [SerializeField] TMP_Text NameText;
        [SerializeField] TMP_Text IncomeText;
        [SerializeField] TMP_Text BonusText;
        [SerializeField] TMP_Text DescriptionText;
        [SerializeField] TMP_Text LevelText;
        [SerializeField] TMP_Text UpgradeButtonText;
        [Space]
        [SerializeField] Slider LevelProgressSlider;
        [SerializeField] Image IconImage;
        [SerializeField] Button UpgradeButton;

        [Header("Button GameObjects")]
        [SerializeField] GameObject AddButtonObject;
        [SerializeField] GameObject RemoveButtonObject;

        int _bountyId;

        AggregatedBounty _assignedBounty => App.Bounties.GetBounty(_bountyId);

        public void Set(AggregatedBounty bounty)
        {
            _bountyId = bounty.ID;

            UpdateUI();

            ShowInnerPanel();
        }

        void UpdateUI()
        {
            IconImage.sprite            = _assignedBounty.Icon;
            NameText.text               = _assignedBounty.Name;
            LevelText.text              = $"Lvl. <color=orange>{_assignedBounty.Level}</color>";
            IncomeText.text             = $"Produces <color=white>{_assignedBounty.Income}</color>";
            BonusText.text              = Format.BonusValue(_assignedBounty.BonusType, _assignedBounty.BonusValue);
            DescriptionText.text        = _assignedBounty.Description;
            LevelProgressSlider.value   = !_assignedBounty.IsMaxLevel ? (_assignedBounty.NumDefeats / (float)_assignedBounty.Levels[_assignedBounty.Level].NumDefeatsRequired) : 1.0f;
            UpgradeButtonText.text      = _assignedBounty.CanUpgrade ? "Upgrade" : _assignedBounty.IsMaxLevel ? "Max level" : "Not yet...";
            UpgradeButton.interactable  = _assignedBounty.CanUpgrade;
        }

        public void OnAddButton()
        {

        }

        public void OnRemoveButton()
        {

        }

        public void OnUpgradeButton()
        {
            App.Bounties.UpgradeBounty(_assignedBounty.ID, (success, resp) =>
            {
                UpdateUI();

                if (!success)
                {
                    GMLogger.Error(resp.Message);
                }
            });
        }
    }
}
