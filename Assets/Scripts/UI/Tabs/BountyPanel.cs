
using UnityEngine;
using UnityEngine.UI;

namespace GM.Bounty.UI
{
    using GM.UI;
    public class BountyPanel : PanelController
    {
        [Header("Components")]
        [SerializeField] Text bountyIncomeText;
        [SerializeField] Text unclaimedTotalText;
        [SerializeField] Text bountyPointsText;
        [Space]
        [SerializeField] Slider bountySlider;


        protected override void PeriodicUpdate()
        {
            UpdatePointsCollection();
        }


        protected override void OnShown()
        {
            UpdatePointsCollection();
        }


        void UpdatePointsCollection()
        {
            BountySnapshot snapshot = UserData.Get.Bounties.CreateSnapshot();

            bountyPointsText.text   = FormatString.Number(UserData.Get.Inventory.BountyPoints);
            bountyIncomeText.text   = $"{snapshot.HourlyIncome} / hour (Max {snapshot.Capacity})";
            unclaimedTotalText.text = $"Collect ({snapshot.Unclaimed})";

            bountySlider.value = snapshot.PercentFilled;
        }


        // = = = Button Callbacks = = = //

        public void OnClaimPoints()
        {
            UserData.Get.Bounties.ClaimPoints(() => { UpdatePointsCollection(); });
        }
    }
}
