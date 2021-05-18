using UnityEngine;
using UnityEngine.UI;

namespace GM.Bounty
{
    using BountySO      = GreedyMercs.BountySO;
    using StaticData    = GreedyMercs.StaticData;

    public class BountyObject : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] Text pointsText;
        [SerializeField] Text bountyName;

        [SerializeField] Image icon;

        public void SetBounty(BountyState state)
        {
            BountySO data = StaticData.BountyList.Get(state.bountyId);

            bountyName.text = data.name;
            pointsText.text = string.Format("{0} Points / hour", data.hourlyIncome);

            icon.sprite = ResourceManager.LoadSprite("BountyIcons", data.iconString);
        }
    }
}