using UnityEngine;
using UnityEngine.UI;

namespace BountyUI
{
    using BountyData;

    public class BountyIcon : MonoBehaviour
    {
        BountyID bounty;

        [Header("Components")]
        [SerializeField] Text pointsText;
        [SerializeField] Text bountyName;
        [SerializeField] Text levelText;

        [SerializeField] Image icon;

        public void SetBounty(BountySO scriptableBounty)
        {
            bounty = scriptableBounty.BountyID;

            bountyName.text = scriptableBounty.name;

            pointsText.text = string.Format("{0} Points / hour", Formulas.CalcBountyHourlyIncome(scriptableBounty.BountyID));

            icon.sprite = scriptableBounty.icon;
        }

        void FixedUpdate()
        {
            levelText.text = "Level " + GameState.Bounties.GetState(bounty).level;
        }
    }
}