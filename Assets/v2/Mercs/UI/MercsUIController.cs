using System.Collections.Generic;
using UnityEngine;
using MercID = GM.Common.Enums.MercID;

namespace GM.Mercs.UI
{
    public class MercsUIController : GM.UI.Panels.PanelController
    {
        [Header("Prefabs")]
        public GameObject SquadMercSlotObject;
        public GameObject ManageMercsObject;

        [Header("References")]
        public GM.UI.AmountSelector UpgradeAmountSelector;
        [Space]
        public Transform AvailMercSlotsParent;
        public Transform SquadMercSlotsParent;
        [Space]

        // ...
        Dictionary<MercID, MercUIObject> MercSlots = new Dictionary<MercID, MercUIObject>();

        void Start()
        {
            SubscribeToEvents();
            UpdateSlotsUI();
        }

        void SubscribeToEvents()
        {
            ISquadController squad = this.GetComponentInScene<ISquadController>();

            squad.E_MercAddedToSquad.AddListener(() => UpdateSlotsUI());
        }

        void UpdateSlotsUI()
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

        void InstantiateSlot(MercID merc)
        {
            SquadMercSlot slot = Instantiate<SquadMercSlot>(SquadMercSlotObject, SquadMercSlotsParent);

            slot.Assign(merc, UpgradeAmountSelector);

            MercSlots.Add(merc, slot);
            
        }

        void DestroySlot(MercID mercId)
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
            InstantiateUI<MercManagePopup>(ManageMercsObject).AssignCallback(() => {
                UpdateSlotsUI();
            });
        }
    }
}