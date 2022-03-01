using GM.Common.Enums;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using System;

namespace GM.Mercs.UI
{
    public class MercManagePopup : GM.Core.GMMonoBehaviour
    {
        [Header("Prefab Objects")]
        public GameObject MercSlot;

        [Header("Parents")]
        public Transform MercSquadParent;
        public Transform AvailableMercsParent;

        // Scene
        MercSquadController MercSquad;

        public bool SquadFull => SquadMercs.Count >= GM.Common.Constants.MAX_SQUAD_SIZE;

        Dictionary<UnitID, MercManageSlot> Slots = new Dictionary<UnitID, MercManageSlot>();
        List<MercManageSlot> SquadMercs => Slots.Values.Where(x => x.InSquad).ToList();


        Action OnSavedChanges;

        public void AssignCallback(Action callback)
        {
            OnSavedChanges = callback;
        }


        void Awake()
        {
            MercSquad = this.GetComponentInScene<MercSquadController>();
        }

        void Start()
        {
            foreach (var unlockedMerc in App.Data.Mercs.UnlockedMercs)
            {
                MercManageSlot slot = Instantiate<MercManageSlot>(MercSlot, null);

                UpdateParent(slot, unlockedMerc.InDefaultSquad);

                slot.Set(this, unlockedMerc.ID);

                Slots[unlockedMerc.ID] = slot;
            }
        }

        void UpdateParent(MercManageSlot slot, bool inSquad)
        {
            if (inSquad)
            {
                slot.transform.SetParent(MercSquadParent, false);
            }
            else
            {
                slot.transform.SetParent(AvailableMercsParent, false);
            }
        }

        public void UpdateMerc(UnitID unit, bool inSquad)
        {
            MercManageSlot slot = Slots[unit];

            UpdateParent(slot, inSquad);

            Slots.Values.ToList().ForEach(x => x.UpdateActiveUI());
        }

        // = UI Callbacks = //

        public void Button_SaveChanges()
        {
            List<UnitID> newSquadMercIds = Slots.Where(x => x.Value.InSquad).Select(x => x.Key).ToList();

            foreach (UnitID merc in App.Data.Mercs.MercsInSquad)
            {
                MercSquad.RemoveMercFromSquad(merc);
                App.Data.Mercs.RemoveMercFromSquad(merc);
            }

            foreach (UnitID merc in newSquadMercIds)
            {
                MercSquad.AddMercToSquad(merc);
                App.Data.Mercs.AddMercToSquad(merc);
            }

            OnSavedChanges?.Invoke();

            Destroy(gameObject);
        }
    }
}
