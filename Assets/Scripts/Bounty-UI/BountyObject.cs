using UnityEngine;
using UnityEngine.UI;

namespace GM.Bounty
{
    using GM.Data;
    using GM.Bounty;

    public class BountyObject : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] Text pointsText;
        [SerializeField] Text bountyName;

        [SerializeField] Image icon;

        int _bountyId;

        public void SetBounty(int bountyId)
        {
            _bountyId = bountyId;

            BountyData data = GetData();

            bountyName.text = data.Name;
            pointsText.text = string.Format("{0}", data.HourlyIncome);

            icon.sprite = data.Icon;
        }

        BountyData GetData() => GameData.Get.Bounties.Get(_bountyId);
    }
}