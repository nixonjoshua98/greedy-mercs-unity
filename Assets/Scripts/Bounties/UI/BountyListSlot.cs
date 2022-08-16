using SRC.Bounties.Models;
using SRC.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SRC.Bounties.UI
{
    public class BountyListSlot : SRC.Core.GMMonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] GameObject PopupPanelObject;

        [Header("Text Elements")]
        [SerializeField] TMP_Text NameText;
        [SerializeField] TMP_Text PointsText;

        [Header("Progress Slider")]
        [SerializeField] Slider UpgradeProgressSlider;
        [SerializeField] Image UpgradeProgressSliderFill;
        [Space]
        [SerializeField] RarityIcon Icon;

        public AggregatedBounty Bounty;

        public void Initialize(AggregatedBounty bounty)
        {
            Bounty = bounty;

            UpdateUI();
        }

        private void UpdateUI()
        {
            NameText.text = Bounty.Name;
            UpgradeProgressSlider.value = Bounty.NextUpgradeProgressPercentage;
            PointsText.text = Bounty.PointsPerHour.ToString();

            if (Bounty.IsMaxLevel)
            {
                UpgradeProgressSliderFill.color = SRC.Common.Constants.Colors.Orange;
            }

            Icon.Initialize(Bounty);
        }

        public void ShowPopupPanel()
        {
            var popup = UI.Instantiate<BountyPopup>(PopupPanelObject, UILayer.ONE);

            popup.Initialize(Bounty);
            popup.E_OnBountyUpgraded.AddListener(UpdateUI);
        }
    }
}
