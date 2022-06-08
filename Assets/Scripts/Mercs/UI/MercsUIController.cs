using System.Collections.Generic;
using UnityEngine;
using MercID = GM.Common.Enums.MercID;

namespace GM.Mercs.UI
{
    public class MercsUIController : GM.Core.GMMonoBehaviour
    {
        [Header("Prefabs")]
        public GameObject SquadMercSlotObject;
        public GameObject ManageMercsObject;

        [Header("References")]
        public GM.UI.AmountSelector UpgradeAmountSelector;
        [Space]
        public Transform SquadMercSlotsParent;
        [Space]
        private readonly

        // ...
        Dictionary<MercID, MercUIObject> MercSlots = new Dictionary<MercID, MercUIObject>();

        private void Awake()
        {
            SubscribeToEvents();
        }

        private void Start()
        {
            UpdateSlotsUI();
        }

        private void SubscribeToEvents()
        {
            MercSquadController squad = this.GetComponentInScene<MercSquadController>();

            squad.E_MercAddedToSquad.AddListener(() => UpdateSlotsUI());
        }

        private void UpdateSlotsUI()
        {
            foreach (var merc in App.Mercs.UnlockedMercs)
            {
                // Merc has been removed from squad
                if (MercSlots.ContainsKey(merc.ID) && !merc.InSquad)
                {
                    DestroySlot(merc.ID);
                }

                // Merc has been added to the squad
                else if (!MercSlots.ContainsKey(merc.ID) && merc.InSquad)
                {
                    InstantiateSlot(merc.ID);
                }
            }
        }

        private void InstantiateSlot(MercID merc)
        {
            SquadMercSlot slot = this.Instantiate<SquadMercSlot>(SquadMercSlotObject, SquadMercSlotsParent);

            slot.Assign(merc, UpgradeAmountSelector);

            MercSlots.Add(merc, slot);

        }

        private void DestroySlot(MercID mercId)
        {
            if (MercSlots.TryGetValue(mercId, out MercUIObject slot))
            {
                Destroy(slot.gameObject);
                MercSlots.Remove(mercId);
            }
        }

        // = UI Callbacks = //

        public void ShowAvailableMercs()
        {
            this.InstantiateUI<MercManagePopup>(ManageMercsObject).AssignCallback(() =>
            {
                UpdateSlotsUI();
            });
        }
    }
}