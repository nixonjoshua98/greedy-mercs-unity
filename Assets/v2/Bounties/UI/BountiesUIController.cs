using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GM.Bounties.UI
{
    public class BountiesUIController : GM.Core.GMMonoBehaviour
    {
        [Header("Prefabs")]
        public GameObject BountySlotObject;

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


        private readonly Dictionary<int, BountySlot> slots = new();

        // Backing Fields
        bool _IsClaimingPoints;

        /// <summary>
        /// Property which includes an update to the relevant UI upon value change
        /// </summary>
        bool IsClaimingPoints { get => _IsClaimingPoints; set  { _IsClaimingPoints = value; UpdateClaimButton(); } }

        private void OnEnable()
        {
            UpdateBountySlots();
        }

        private void Start()
        {
            StartCoroutine(_InternalLoop());
        }

        private IEnumerator _InternalLoop()
        {
            while (true)
            {
                UpdateClaimUI();

                yield return new WaitForSecondsRealtime(0.25f);
            }
        }

        private void UpdateBountySlots()
        {
            var unlockedBounties = App.Bounties.UnlockedBounties;

            for (int i = 0; i < unlockedBounties.Count; i++)
            {
                var bounty = unlockedBounties[i];

                if (!slots.TryGetValue(bounty.ID, out BountySlot slot))
                {
                    slot = slots[bounty.ID] = Instantiate<BountySlot>(BountySlotObject, BountySlotParent);

                    slot.Assign(bounty.ID);
                }

                slot.transform.SetSiblingIndex(i);
            }
        }

        private void UpdateClaimUI()
        {
            ClaimAmountText.text = $"{App.Bounties.TotalUnclaimedPoints}/{App.Bounties.MaxClaimPoints}";
            ClaimSlider.value = App.Bounties.ClaimPercentFilled;
        }

        void UpdateClaimButton()
        {
            ClaimButton.interactable = !IsClaimingPoints;
            ClaimButtonTextTypeWriter.enabled = IsClaimingPoints;

            if (!IsClaimingPoints)
                ClaimButtonText.text = "Claim";
        }

        public void OnClaimButton()
        {
            IsClaimingPoints = true;

            App.Bounties.ClaimPoints((success, resp) =>
            {
                IsClaimingPoints = false;

                UpdateClaimUI();

                if (!success)
                {
                    Modals.ShowServerError(resp);
                }
            });
        }
    }
}
