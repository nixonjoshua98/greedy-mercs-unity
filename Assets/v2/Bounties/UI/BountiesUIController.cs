using TMPro;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using StopWatch = System.Diagnostics.Stopwatch;

namespace GM.Bounties.UI
{
    public class BountiesUIController : GM.UI.Panels.Panel
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
        List<BountySlot> activeBountySlots = new List<BountySlot>();
        

        void Awake()
        {
            sliderUpdateTimer = StopWatch.StartNew();

            UpdateUI(force: true);

            SetActiveButtons(isEditMode: false);

            InstantiateBountySlots();
        }

        void FixedUpdate()
        {
            UpdateUI();
        }

        void InstantiateBountySlots()
        {
            foreach (var bounty in App.Data.Bounties.UnlockedBountiesList)
            {
                if (!slots.ContainsKey(bounty.Id))
                {
                    var slot = Instantiate<BountySlot>(BountySlotObject, BountySlotParent);

                    slot.Assign(bounty.Id);

                    slots[bounty.Id] = slot;
                }
            }

            BountiesLayout.UpdateCellSize();
        }

        void UpdateUI(bool force = false)
        {
            HeaderText.text = $"Active Bounties ({(isEditing ? activeBountySlots.Count : App.Data.Bounties.ActiveBountiesList.Count)}/{App.Data.Bounties.MaxActiveBounties})";

            if (force || sliderUpdateTimer.ElapsedMilliseconds >= 500)
            {
                sliderUpdateTimer.Restart();

                TimeUntilMaxClaimText.text = $"Full in <color=orange>{App.Data.Bounties.TimeUntilMaxUnclaimedHours.Format()}</color>";
                ClaimAmountText.text = $"{Format.Number(App.Data.Bounties.TotalUnclaimedPoints)}/{Format.Number(App.Data.Bounties.MaxClaimPoints)}";

                ClaimSlider.value = App.Data.Bounties.ClaimPercentFilled;
                ClaimSliderFill.color = Color.Lerp(Common.Colors.Red, Common.Colors.Green, ClaimSlider.value);
            }
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

            activeBountySlots.Clear();

            foreach (var slot in slots.Values)
            {
                slot.StartEdit(OnBountySelected);

                if (slot.AssignedBounty.IsActive)
                {
                    activeBountySlots.Add(slot);
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

        // == Callbacks == //
        public void OnBountySelected(UI.BountySlot slot)
        {
            if (activeBountySlots.Contains(slot))
            {
                activeBountySlots.Remove(slot);
                slot.SetSelected(false);
            }
            else if (activeBountySlots.Count < App.Data.Bounties.MaxActiveBounties)
            {
                activeBountySlots.Add(slot);
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
            List<int> ids = activeBountySlots.Select(slot => slot.AssignedBounty.Id).ToList();

            App.Data.Bounties.SetActiveBounties(ids, (success, resp) =>
            {
                if (success)
                {
                    StopEditMode();
                }
            });
        }

        public void OnClaimButton()
        {
            App.Data.Bounties.ClaimPoints((success, resp) => {
                UpdateUI(force: true);
            });
        }
    }
}
