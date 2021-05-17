using UnityEngine;
using UnityEngine.UI;

namespace GreedyMercs
{
    public class BountyIcon : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] Text pointsText;
        [SerializeField] Text bountyName;

        [SerializeField] Image icon;

        public void SetBounty(BountySO scriptableBounty)
        {
            bountyName.text = scriptableBounty.name;

            pointsText.text = string.Format("{0} Points / hour", Formulas.CalcBountyHourlyIncome(scriptableBounty.BountyID));

            icon.sprite = scriptableBounty.icon;
        }
    }
}