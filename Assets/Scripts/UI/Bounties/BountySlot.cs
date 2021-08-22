using UnityEngine;
using UnityEngine.UI;

namespace GM.Bounty.UI
{
    using GM.Data;
    using GM.Bounty;

    public class BountySlot : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] Text pointsText;
        [SerializeField] Text bountyName;

        [SerializeField] Image icon;

        public void SetBounty(int bountyId)
        {
            BountyData data = GameData.Get.Bounties.Get(bountyId);

            bountyName.text = data.Name.ToUpper();
            pointsText.text = string.Format("{0}", data.HourlyIncome);

            icon.sprite = data.Icon;
        }
    }
}