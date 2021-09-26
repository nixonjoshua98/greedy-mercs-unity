using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using GM.Bounties.Data;



namespace GM.Bounties.UI
{
    [System.Serializable]
    struct BountyListViewButtons
    {
        public GameObject Confirm;
        public GameObject Edit;
        public GameObject Cancel;

        public void SetActive(bool confirm, bool edit, bool cancel)
        {
            Confirm.SetActive(confirm);
            Edit.SetActive(edit);
            Cancel.SetActive(cancel);
        }
    }


    public class BountyListView : GM.UI.PanelController
    {
        [SerializeField] BountyListViewButtons buttons;
        [Space]
        [SerializeField] Transform activeBountiesParent;
        [SerializeField] Transform availableBountiesParent;

        Dictionary<int, BountySlot> activeSlots;
        Dictionary<int, BountySlot> availableSlots;

        void Awake()
        {
            activeSlots = new Dictionary<int, BountySlot>();
            availableSlots = new Dictionary<int, BountySlot>();

            SetupBountySlots();
        }


        protected override void OnShown()
        {
            SetupBountySlots();
        }


        protected override void OnHidden()
        {
            DestroyAllSlots();
        }


        void CreateBountySlots()
        {
            foreach (UserBountyState bounty in App.Data.Bounties.User.UnlockedBounties)
            {
                GameBountyData data = App.Data.Bounties.Game[bounty.ID];

                BountySlot slot;

                if (bounty.IsActive)
                {
                    slot = CanvasUtils.Instantiate<BountySlot>(data.Slot.gameObject, activeBountiesParent);

                    activeSlots.Add(data.ID, slot);
                }

                else
                {
                    slot = CanvasUtils.Instantiate<BountySlot>(data.Slot.gameObject, availableBountiesParent);

                    availableSlots.Add(data.ID, slot);
                }

                slot.SetBounty(data.ID);
            }
        }


        void SetupBountySlots()
        {
            DestroyAllSlots();
            CreateBountySlots();
            SwitchToIdleMode();
        }


        void DestroyAllSlots()
        {
            foreach (BountySlot slot in availableSlots.Values)
                Destroy(slot.gameObject);

            foreach (BountySlot slot in activeSlots.Values)
                Destroy(slot.gameObject);

            activeSlots.Clear();
            availableSlots.Clear();
        }


        void SetSlotToActive(BountySlot slot)
        {
            slot.Animator.Play("Jiggle");

            slot.E_OnClick = OnActiveSlotClick;
        }


        void SetSlotToAvailable(BountySlot slot)
        {
            slot.Animator.Play("Jiggle");

            slot.E_OnClick = OnAvailableSlotClick;
        }


        void SetSlotToIdle(BountySlot slot)
        {
            slot.Animator.Play("Idle");

            slot.E_OnClick = null;
        }


        public void SwitchToIdleMode()
        {
            foreach (BountySlot slot in availableSlots.Values)
                SetSlotToIdle(slot);

            foreach (BountySlot slot in activeSlots.Values)
                SetSlotToIdle(slot);

            buttons.SetActive(false, true, false);
        }


        // = = = Button Callbacks = = = //
        public void CancelChanges()
        {
            SwitchToIdleMode();
            SetupBountySlots();
        }


        public void ConfirmChanges()
        {
            SwitchToIdleMode();

            List<int> ids = activeSlots.Values.Select(s => s.BountyID).ToList();

            App.Data.Bounties.SetActiveBounties(ids, (success) => {

                SetupBountySlots();

                if (!success)
                {
                    CanvasUtils.ShowInfo("Active Bounties", "Failed to update active bounties");
                }

            });
        }


        public void SwitchToEditMode()
        {
            foreach (BountySlot slot in availableSlots.Values)
                SetSlotToAvailable(slot);

            foreach (BountySlot slot in activeSlots.Values)
                SetSlotToActive(slot);

            buttons.SetActive(true, false, true);
        }


        // = = = Slot Callbacks = = = //
        void OnActiveSlotClick(int selectedBountyID)
        {
            if (activeSlots.TryGetValue(selectedBountyID, out BountySlot slot))
            {
                SetSlotToAvailable(slot);

                activeSlots.Remove(selectedBountyID);
                availableSlots.Add(selectedBountyID, slot);

                slot.transform.SetParent(availableBountiesParent);
            }
        }


        void OnAvailableSlotClick(int selectedBountyID)
        {
            if (activeSlots.Count < App.Data.Bounties.Game.MaxActiveBounties)
            {
                if (availableSlots.TryGetValue(selectedBountyID, out BountySlot slot))
                {
                    SetSlotToActive(slot);

                    availableSlots.Remove(selectedBountyID);
                    activeSlots.Add(selectedBountyID, slot);

                    slot.transform.SetParent(activeBountiesParent);
                }
            }
        }
    }
}
