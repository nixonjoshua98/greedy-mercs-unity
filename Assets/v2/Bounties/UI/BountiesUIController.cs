using GM.Bounties.Data;
using GM.Common;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using StopWatch = System.Diagnostics.Stopwatch;

namespace GM.Bounties.UI
{
    public class BountiesUIController : GM.Core.GMMonoBehaviour
    {
        [Header("Prefabs")]
        public GameObject BountySlotObject;

        [Header("References")]
        public Button EditButton;
        public Button CancelButton;
        public Button ConfirmButton;
        [Space]
        public TMP_Text ClaimAmountText;
        public TMP_Text TimeUntilMaxClaimText;
        public Text HeaderText;
        [Space]
        public Slider ClaimSlider;
        public Image ClaimSliderFill;
        public Transform BountySlotParent;
        public GM.UI.Layouts.ExpandableGridLayout BountiesLayout;

        bool isEditing = false;

        StopWatch sliderUpdateTimer;

        Dictionary<int, BountySlot> slots = new Dictionary<int, BountySlot>();

        List<int> activeBountyIds = new List<int>();


        void Awake()
        {
            sliderUpdateTimer = StopWatch.StartNew();

            UpdateUI();

            SetActiveButtons(isEditMode: false);

            UpdateBountySlots();
        }

        void FixedUpdate()
        {
            UpdateUI();

            if (sliderUpdateTimer.ElapsedMilliseconds >= 500)
            {
                UpdateClaimUI();

                sliderUpdateTimer.Restart();
            }
        }

        void UpdateBountySlots()
        {
            List<AggregatedBounty> bounties = App.Bounties.UnlockedBountiesList.OrderBy(b => b.IsActive ? 0 : 1).ThenBy(b => b.Id).ToList();

            for (int i = 0; i < bounties.Count; ++i)
            {
                AggregatedBounty bounty = bounties[i];

                if (!slots.TryGetValue(bounty.Id, out BountySlot slot))
                {
                    slot = slots[bounty.Id] = Instantiate<BountySlot>(BountySlotObject, BountySlotParent);

                    slot.Assign(bounty.Id);
                }

                slot.transform.SetSiblingIndex(i);
            }

            BountiesLayout.UpdateCellSize();
        }

        void UpdateUI()
        {
            HeaderText.text = $"Active Bounties ({(isEditing ? activeBountyIds.Count : App.Bounties.ActiveBountiesList.Count)}/{App.Bounties.MaxActiveBounties})";
        }

        void UpdateClaimUI()
        {
            TimeUntilMaxClaimText.text = $"Full in <color=orange>{App.Bounties.TimeUntilMaxUnclaimedHours.Format()}</color>";
            ClaimAmountText.text = $"{Format.Number(App.Bounties.TotalUnclaimedPoints)}/{Format.Number(App.Bounties.MaxClaimPoints)}";

            ClaimSlider.value = App.Bounties.ClaimPercentFilled;
            ClaimSliderFill.color = Color.Lerp(Constants.Colors.Red, Constants.Colors.Green, ClaimSlider.value);
        }

        void SetActiveButtons(bool isEditMode)
        {
            EditButton.gameObject.SetActive(!isEditMode);
            CancelButton.gameObject.SetActive(isEditMode);
            ConfirmButton.gameObject.SetActive(isEditMode);
        }

        void StartEditMode()
        {
            isEditing = true;

            SetActiveButtons(true);

            activeBountyIds.Clear();

            foreach (var slot in slots.Values)
            {
                slot.StartEdit(OnBountySelected);

                if (slot.AssignedBounty.IsActive)
                {
                    activeBountyIds.Add(slot.AssignedBounty.Id);
                }
            }
        }

        void StopEditMode()
        {
            isEditing = false;

            SetActiveButtons(false);

            foreach (var slot in slots.Values)
            {
                slot.StopEdit();
            }
        }

        public void OnBountySelected(BountySlot slot)
        {
            int bountyId = slot.AssignedBounty.Id;

            if (activeBountyIds.Contains(bountyId))
            {
                activeBountyIds.Remove(bountyId);
                slot.SetSelected(false);
            }

            else if (activeBountyIds.Count < App.Bounties.MaxActiveBounties)
            {
                activeBountyIds.Add(bountyId);
                slot.SetSelected(true);
            }
        }

        public void OnEditButton()
        {
            StartEditMode();
        }

        public void OnCancelButton()
        {
            StopEditMode();
        }

        public void OnConfirmButton()
        {
            App.Bounties.SetActiveBounties(activeBountyIds, (success, resp) =>
            {
                if (success)
                {
                    StopEditMode();
                }

                UpdateBountySlots();
            });
        }

        public void OnClaimButton()
        {
            App.Bounties.ClaimPoints((success, resp) =>
            {
                UpdateClaimUI();
            });
        }
    }
}
