using GM.Bounties.Models;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GM.Bounties.UI
{
    public class BountyPopup : GM.UI.PopupBase
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

        [Header("Buttons")]
        [SerializeField] Button AddButton;
        [SerializeField] Button RemoveButton;
        [SerializeField] Button UpgradeButton;

        int _bountyId;

        AggregatedBounty _assignedBounty => App.Bounties.GetBounty(_bountyId);

        public void Initialize(int bountyId)
        {
            _bountyId = bountyId;

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

            UpdateActionButtons();
        }

        void UpdateActionButtons()
        {
            AddButton.gameObject.SetActive(!_assignedBounty.IsActive);
            RemoveButton.gameObject.SetActive(_assignedBounty.IsActive);
        }

        /* Event Listeners */

        public void OnCloseButton()
        {
            Destroy(gameObject);
        }

        public void OnAddButton()
        {
            App.Bounties.AddActiveBounty(_bountyId, (success, resp) =>
            {
                UpdateUI();

                if (!success)
                    GMLogger.Error(resp.Message);
            });
        }

        public void OnRemoveButton()
        {
            App.Bounties.RemoveActiveBounty(_bountyId, (success, resp) =>
            {
                UpdateUI();

                if (!success)
                    GMLogger.Error(resp.Message);
            });
        }

        public void OnUpgradeButton()
        {
            App.Bounties.UpgradeBounty(_bountyId, (success, resp) =>
            {
                UpdateUI();

                if (!success)
                    GMLogger.Error(resp.Message);
            });
        }
    }
}
