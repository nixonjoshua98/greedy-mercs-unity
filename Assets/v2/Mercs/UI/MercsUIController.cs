using System.Collections.Generic;
using UnityEngine;
using UnitID = GM.Common.Enums.UnitID;

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
        Dictionary<UnitID, MercUIObject> MercSlots = new Dictionary<UnitID, MercUIObject>();

        void Start()
        {
            UpdateSlotsUI();
        }

        void UpdateSlotsUI()
        {
            foreach (var merc in App.GMData.Mercs.UnlockedMercs)
            {
                // Merc has been removed from squad
                if (MercSlots.ContainsKey(merc.ID) && !merc.InDefaultSquad)
                {
                    DestroySlot(merc.ID);
                }

                // Merc has been added to the squad
                else if (!MercSlots.ContainsKey(merc.ID) && merc.InDefaultSquad)
                {
                    InstantiateSlot(merc.ID);
                }
            }
        }

        void InstantiateSlot(UnitID merc)
        {
            SquadMercSlot slot = Instantiate<SquadMercSlot>(SquadMercSlotObject, SquadMercSlotsParent);

            slot.Assign(merc, UpgradeAmountSelector);

            MercSlots.Add(merc, slot);
            
        }

        void DestroySlot(UnitID mercId)
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