using UnityEngine;
using UnityEngine.UI;

namespace GreedyMercs
{
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
            BountySO staticData = StaticData.BountyList.Get(bounty);
            UpgradeState state  = GameState.Bounties.GetState(bounty);
            
            levelText.text = "Level " + (staticData.maxLevel == state.level ? "MAX" : state.level.ToString());
        }
    }
}