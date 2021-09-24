﻿
using UnityEngine;
using UnityEngine.UI;

namespace GM.Bounties.UI
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
        [Space]
        [SerializeField] Button claimButton;
        [SerializeField] Animator claimButtonAnim;

        [Header("Objects")]
        [SerializeField] GameObject claimPopupText;


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

            claimButtonAnim.Play(snapshot.PercentFilled >= 0.5f && snapshot.Capacity > 0 ? "Pulse" : "Idle");
        }


        // = = = Button Callbacks = = = //

        public void OnClaimPoints()
        {
            UserData.Get.Bounties.ClaimPoints((success, claimed) => { 
                UpdatePointsCollection();

                claimButtonAnim.Play("Idle");

                if (success)
                {
                    ItemTextPopup popup = CanvasUtils.Instantiate<ItemTextPopup>(claimPopupText, claimButton.transform.position);

                    popup.Setup(GM.Items.Data.ItemType.BOUNTY_POINTS, $"{FormatString.Number(claimed)}");
                }
            });
        }
    }
}
