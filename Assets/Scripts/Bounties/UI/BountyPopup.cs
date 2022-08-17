using SRC.Bounties.Models;
using SRC.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

namespace SRC.Bounties.UI
{
    public class BountyPopup : SRC.UI.PopupBase
    {
        [Header("Text Elements")]
        [SerializeField] private TMP_Text PointsPerHourText;
        [SerializeField] private TMP_Text BonusText;
        [SerializeField] private TMP_Text DescriptionText;
        [SerializeField] private TMP_Text UpgradeButtonText;
        [Space]
        [SerializeField] private RarityIcon Icon;
        [SerializeField] private Button UpgradeButton;

        [Header("Events")]
        [HideInInspector] public readonly UnityEvent E_OnBountyUpgraded = new();
        private AggregatedBounty Bounty;

        public void Initialize(AggregatedBounty bounty)
        {
            Bounty = bounty;

            UpdateUI();

            ShowInnerPanel();
        }

        private void FixedUpdate()
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            Icon.Initialize(Bounty);

            PointsPerHourText.text = Bounty.PointsPerHour.ToString();
            BonusText.text = Format.BonusValue(Bounty.BonusType, Bounty.BonusValue);
            DescriptionText.text = Bounty.Description;

            UpgradeButton.interactable = Bounty.CanUpgrade;

            if (Bounty.IsMaxLevel)
            {
                UpgradeButtonText.text = "Max Level";
            }
            else
            {
                UpgradeButtonText.text = $"{Bounty.CurrentKillCount} / {Bounty.NextLevel.KillsRequired} Kills";
            }
        }

        /* Event Listeners */

        public void OnCloseButton()
        {
            Destroy(gameObject);
        }

        public void OnUpgradeButton()
        {
            App.Bounties.UpgradeBounty(Bounty.BountyID, resp =>
            {
                if (resp.IsSuccess)
                {
                    E_OnBountyUpgraded.Invoke();
                }
            });
        }
    }
}
