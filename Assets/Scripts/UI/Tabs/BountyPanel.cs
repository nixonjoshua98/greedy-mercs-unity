
using UnityEngine;
using UnityEngine.UI;

namespace GM.Bounties.UI
{
    using GM.UI;
    public class BountyPanel : CloseablePanel
    {
        [Header("Prefabs")]
        [SerializeField] GameObject BountyObjectSlot;

        [Header("Objects")]
        [SerializeField] Transform bountySlotsParent;

        [Header("Components")]
        [SerializeField] Text bountyIncomeText;
        [SerializeField] Text unclaimedTotalText;
        [SerializeField] Text bountyPointsText;
        [Space]
        [SerializeField] Slider bountySlider;

        void Start()
        {
            InstantiateIcons();
        }


        protected override void PeriodicUpdate()
        {
            BountySnapshot snapshot = UserData.Get().Bounties.CreateSnapshot();

            bountyPointsText.text   = UserData.Get().Inventory.BountyPoints.ToString();
            bountyIncomeText.text   = $"{snapshot.HourlyIncome} / hour (Max {snapshot.Capacity})";
            unclaimedTotalText.text = string.Format("Collect ({0})", snapshot.Unclaimed);

            bountySlider.value = snapshot.PercentFilled;
        }



        void InstantiateIcons()
        {
            foreach (BountyState bounty in UserData.Get().Bounties.StatesList)
            {
                GameObject inst = CanvasUtils.Instantiate(BountyObjectSlot, bountySlotsParent);

                BountyObject obj = inst.GetComponent<BountyObject>();

                obj.SetBounty(bounty.ID);
            }
        }


        // = = = Button Callbacks = = =
        public void OnClaimPoints()
        {
            UserData.Get().Bounties.ClaimPoints(() => { });
        }
    }
}
