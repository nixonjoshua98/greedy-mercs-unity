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

        public void SetBounty(int bountyId)
        {
            BountyDataStruct data = StaticData.Bounty.Get(bountyId);

            bountyName.text = data.Name;
            pointsText.text = string.Format("{0}", data.HourlyIncome);

            icon.sprite = data.Icon;
        }
    }
}