using GM.Mercs.Data;
using System.Collections.Generic;
using UnityEngine;
using MercID = GM.Common.Enums.MercID;

namespace GM.Mercs.UI
{
    public class MercsUIController : GM.UI.Panels.PanelController
    {
        [Header("Prefabs")]
        public GameObject SquadMercSlotObject;
        public GameObject AvailMercSlotObject;

        [Header("References")]
        public GM.UI.AmountSelector UpgradeAmountSelector;
        [Space]
        public Transform AvailMercSlotsParent;
        public Transform SquadMercSlotsParent;
        [Space]
        
        // Scene
        MercSquadController MercSquad;

        // ...
        Dictionary<MercID, MercUIObject> MercSlots = new Dictionary<MercID, MercUIObject>();

        void Awake()
        {
            MercSquad = this.GetComponentInScene<MercSquadController>();
        }

        void Start()
        {
            InstantiateMercSlots();
        }

        void InstantiateMercSlots()
        {
            App.Data.Mercs.UnlockedMercs.ForEach(merc => InstantiateSlot(merc.Id));
        }

        void InstantiateSlot(MercID mercId)
        {
            bool isMercUnlocked = App.Data.Mercs.IsMercUnlocked(mercId);

            if (isMercUnlocked)
            {
                DestroySlot(mercId); // Destroy if exists

                MercData merc = App.Data.Mercs.GetMerc(mercId);

                if (merc.InDefaultSquad)
                {
                    InstantiateSquadMercSlot(merc.Id);
                }

                else
                {
                    InstantiateIdleMercSlot(merc.Id);
                }
            }

        }

        void InstantiateSquadMercSlot(MercID mercId)
        {
            SquadMercSlot slot = Instantiate<SquadMercSlot>(SquadMercSlotObject, SquadMercSlotsParent);

            slot.Assign(mercId, UpgradeAmountSelector, RemoveMercFromSquad);

            MercSlots.Add(mercId, slot);
        }

        void InstantiateIdleMercSlot(MercID mercId)
        {
            IdleMercSlot slot = Instantiate<IdleMercSlot>(AvailMercSlotObject, AvailMercSlotsParent);

            slot.Assign(mercId, UpgradeAmountSelector, AddSquadToMerc);

            MercSlots.Add(mercId, slot);
        }

        void AddSquadToMerc(MercID mercId)
        {
            if (MercSquad.AddMercToSquad(mercId))
            {
                App.Data.Mercs.AddMercToSquad(mercId);

                InstantiateSlot(mercId);
            }
            else
            {
                GMLogger.Editor("Failed to add unit to squad");
            }
        }

        void RemoveMercFromSquad(MercID mercId)
        {
            if (MercSquad.RemoveMercFromSquad(mercId))
            {
                App.Data.Mercs.RemoveMercFromSquad(mercId);

                InstantiateSlot(mercId);
            }
            else
            {
                GMLogger.Editor("Failed to remove unit from squad");
            }
        }

        void DestroySlot(MercID mercId)
        {
            if (MercSlots.TryGetValue(mercId, out MercUIObject slot))
            {
                Destroy(slot.gameObject);
                MercSlots.Remove(mercId);
            }
        }
    }
}