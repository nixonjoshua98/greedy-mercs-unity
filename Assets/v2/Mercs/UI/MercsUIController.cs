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

        // ...
        Dictionary<MercID, MercUIObject> MercSlots = new Dictionary<MercID, MercUIObject>();

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

                if (merc.InSquad)
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
            MercData data = App.Data.Mercs.GetMerc(mercId);

            bool isSquadFull = App.Data.Mercs.SquadMercs.Count >= Common.Constants.MAX_SQUAD_SIZE;

            if (isSquadFull)
            {
                GMLogger.Editor("Squad max size reached");
            }

            else if (data.InSquad)
            {
                GMLogger.Editor("Merc is already in squad");
            }

            else
            {
                App.Data.Mercs.AddMercToSquad(mercId);

                InstantiateSlot(mercId);
            } 
        }

        void RemoveMercFromSquad(MercID mercId)
        {
            MercData data = App.Data.Mercs.GetMerc(mercId);

            if (!data.InSquad)
            {
                GMLogger.Editor("Merc is not in squad");
            }

            else
            {
                App.Data.Mercs.RemoveMercFromSquad(mercId);

                InstantiateSlot(mercId);
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

        public void OnUnlockButton()
        {
            if (App.Data.Mercs.GetNextHero(out MercID chara))
            {
                GM.Mercs.Models.MercGameDataModel mercData = App.Data.Mercs.GetGameMerc(chara);

                if (App.Data.Inv.Gold >= mercData.UnlockCost)
                {
                    App.Data.Inv.Gold -= mercData.UnlockCost;

                    App.Data.Mercs.UnlockUserMerc(chara);
                }
            }
        }
    }
}