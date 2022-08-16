using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace SRC
{
    public class BountyPointsClaimButton : SRC.Core.GMMonoBehaviour
    {
        [SerializeField] TMP_Text PointsPerHourText;
        [SerializeField] Slider CapacitySlider;

        void FixedUpdate()
        {
            PointsPerHourText.text = $"{App.Bounties.TotalPointsPerHour} / Hour";
            CapacitySlider.value = App.Bounties.ClaimPercentFilled;
        }
    }
}
