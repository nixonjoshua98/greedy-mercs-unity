using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SRC
{
    public class BountyPointsClaimButton : SRC.Core.GMMonoBehaviour
    {
        [SerializeField] private TMP_Text PointsPerHourText;
        [SerializeField] private Slider CapacitySlider;

        private void FixedUpdate()
        {
            PointsPerHourText.text = $"{App.Bounties.TotalPointsPerHour} / Hour";
            CapacitySlider.value = App.Bounties.ClaimPercentFilled;
        }
    }
}
