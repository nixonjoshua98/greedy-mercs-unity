using UnityEngine;
using UnityEngine.UI;

namespace GM.Bounties
{
    using GM.Data;

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

        BountyData GetData() => GameData.Get().Bounties.Get(_bountyId);
    }
}