using SRC.Bounties.Models;
using SRC.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SRC.Bounties.UI
{
    public class BountyPopup : SRC.UI.PopupBase
    {
        [Header("Text Elements")]
        [SerializeField] TMP_Text PointsPerHourText;
        [SerializeField] TMP_Text BonusText;
        [SerializeField] TMP_Text DescriptionText;
        [SerializeField] TMP_Text UpgradeButtonText;
        [Space]
        [SerializeField] RarityIcon Icon;
        [SerializeField] Button UpgradeButton;

        [Header("Events")]
        [HideInInspector] public readonly UnityEvent E_OnBountyUpgraded = new();

        AggregatedBounty Bounty;

        public void Initialize(AggregatedBounty bounty)
        {
            Bounty = bounty;

            Icon.Initialize(Bounty);

            ShowInnerPanel();
        }

        void FixedUpdate()
        {
            PointsPerHourText.text  = Bounty.PointsPerHour.ToString();
            BonusText.text          = Format.BonusValue(Bounty.BonusType, Bounty.BonusValue);
            DescriptionText.text    = Bounty.Description;

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
