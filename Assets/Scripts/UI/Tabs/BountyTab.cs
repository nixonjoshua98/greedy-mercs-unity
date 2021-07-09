using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GM.Bounty
{
    using GM.Inventory;


    public class BountyTab : ExtendedMonoBehaviour
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

            bountyPointsText.text   = InventoryManager.Instance.BountyPoints.ToString();
            bountyIncomeText.text   = string.Format("{0} / hour (Max {1})", snapshot.HourlyIncome, snapshot.Capacity);
            unclaimedTotalText.text = string.Format("Collect ({0})", snapshot.Unclaimed);

            bountySlider.value = snapshot.PercentFilled;
        }

        void InstantiateIcons()
        {
            foreach (BountyState bounty in UserData.Get().Bounties.StatesList)
            {
                GameObject inst = Funcs.UI.Instantiate(BountyObjectSlot, bountySlotsParent, Vector3.zero);

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
