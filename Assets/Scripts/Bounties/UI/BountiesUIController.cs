using GM.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GM.Bounties.UI
{
    public class BountiesUIController : GM.Core.GMMonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] GameObject BountySlotObject;
        [SerializeField] GameObject ActiveBountySlotObject;

        [Space]
        [SerializeField] TMP_Text ClaimAmountText;

        [Header("Claim Button")]
        [SerializeField] Button ClaimButton;
        [SerializeField] TMP_Text ClaimButtonText;
        [SerializeField] TypeWriter ClaimButtonTextTypeWriter;
        [Space]
        [SerializeField] Slider ClaimSlider;
        [SerializeField] Image ClaimSliderFill;
        [SerializeField] Transform BountySlotParent;

        [Header("Parent Transforms")]
        [SerializeField] Transform ActiveBountiesParent;

        private readonly Dictionary<int, BountySlot> slots = new();

        List<ActiveBountySlot> ActiveBountySlots = new();

        // Backing Field
        bool _IsClaimingPoints;

        /// <summary>
        /// Property which includes an update to the relevant UI upon value change
        /// </summary>
        bool IsClaimingPoints { get => _IsClaimingPoints; set { _IsClaimingPoints = value; UpdateClaimUI(); } }

        void Awake()
        {
            InstantiateInitialActiveSlots();
        }

        /// <summary>
        /// Internal update loop to reduce number of unneeded UI updates
        /// </summary>
        private void _InternalLoop()
        {
            UpdateUI();
        }

        void UpdateUI()
        {
            UpdateSlotsUI();
            UpdateClaimUI();
            UpdateActiveSlots();
        }

        void InstantiateInitialActiveSlots()
        {
            for (int i = 0; i < App.Bounties.MaxActiveBounties; i++)
            {
                ActiveBountySlot slot = this.Instantiate<ActiveBountySlot>(ActiveBountySlotObject, ActiveBountiesParent);

                ActiveBountySlots.Add(slot);
            }
        }

        void UpdateActiveSlots()
        {
            var activeBounties = App.Bounties.ActiveBounties;

            for (int i = 0; i < ActiveBountySlots.Count; i++)
            {
                var slot = ActiveBountySlots[i];

                if (activeBounties.Count > i)
                {
                    var bounty = activeBounties[i];

                    if (slot.BountyID != bounty.ID)
                        slot.Initialize(bounty);
                }
                else
                {
                    slot.Reset();
                }
            }
        }

        private void UpdateSlotsUI()
        {
            var unlockedBounties = App.Bounties.UnlockedBounties;

            for (int i = 0; i < unlockedBounties.Count; i++)
            {
                var bounty = unlockedBounties[i];

                if (!slots.TryGetValue(bounty.ID, out BountySlot slot))
                {
                    slot = slots[bounty.ID] = this.Instantiate<BountySlot>(BountySlotObject, BountySlotParent);

                    slot.Initialize(bounty.ID);
                }

                slot.transform.SetSiblingIndex(i);
            }
        }

        private void UpdateClaimUI()
        {
            ClaimAmountText.text = $"{App.Bounties.TotalUnclaimedPoints}/{App.Bounties.MaxClaimPoints}";
            ClaimSlider.value = App.Bounties.ClaimPercentFilled;

            UpdateClaimButton();
        }

        void UpdateClaimButton()
        {
            ClaimButton.interactable = !IsClaimingPoints;
            ClaimButtonTextTypeWriter.enabled = IsClaimingPoints;

            if (!IsClaimingPoints)
                ClaimButtonText.text = "Claim";
        }

        /* Event Listeners */

        public void OnTabSelected()
        {
            InvokeRepeating(nameof(_InternalLoop), 0f, 0.5f);
        }

        public void OnTabDeSelected()
        {
            CancelInvoke();
        }

        public void OnClaimButton()
        {
            IsClaimingPoints = true;

            App.Bounties.ClaimPoints((success, resp) =>
            {
                IsClaimingPoints = false;

                if (!success)
                    GMLogger.Error(resp.Message);
            });
        }
    }
}