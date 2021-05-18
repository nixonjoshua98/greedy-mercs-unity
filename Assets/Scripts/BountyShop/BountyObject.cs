using UnityEngine;
using UnityEngine.UI;

namespace GM.Bounty
{
    using StaticData = GreedyMercs.StaticData;

    public class BountyObject : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] Text pointsText;
        [SerializeField] Text bountyName;

        [SerializeField] Image icon;

        public void SetBounty(BountyState state)
        {
            ServerBountyData data = StaticData.Bounty.Get(state.bountyId);

            bountyName.text = data.name;
            pointsText.text = string.Format("{0} Points / hour", data.hourlyIncome);

            icon.sprite = data.icon;
        }
    }
}