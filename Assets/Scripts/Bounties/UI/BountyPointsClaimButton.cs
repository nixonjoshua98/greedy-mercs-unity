using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SRC
{
    public class BountyPointsClaimButton : SRC.Core.GMMonoBehaviour
    {
        [SerializeField] private TMP_Text PointsPerHourText;
        [SerializeField] private Slider CapacitySlider;

        short _currentLoop;

        private void Awake()
        {
            InvokeRepeating(nameof(UpdateUI), 0.0f, 5.0f);
        }

        private void FixedUpdate()
        {
            CapacitySlider.value = App.Bounties.ClaimPercentFilled;
        }

        private void UpdateUI()
        {
            PointsPerHourText.text = _currentLoop switch
            {
                0 => $"{App.Bounties.TotalPointsPerHour} / Hour",
                1 => $"{App.Bounties.TotalUnclaimedPoints} / {App.Bounties.MaxClaimPoints}",
                _ => ""
            };

            _currentLoop = (short)((_currentLoop + 1) % 2);
        }
    }
}
