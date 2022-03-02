using GM.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GM.Mercs.UI
{
    public class MercManagePopup : GM.Core.GMMonoBehaviour
    {
        [Header("Prefab Objects")]
        public GameObject MercSlot;
        public GameObject ManageMercSlot;
        public GameObject ManageMercIconObject;

        [Header("Parents")]
        public Transform SquadIconsParent;
        public Transform AvailableMercsParent;

        // Scene
        MercSquadController MercSquad;

        public bool SquadFull => SquadMercs.Count >= GM.Common.Constants.MAX_SQUAD_SIZE;

        Dictionary<UnitID, MercManageSlot> Slots = new Dictionary<UnitID, MercManageSlot>();
        List<MercManageIcon> Icons = new List<MercManageIcon>();
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
            CreateIcons();
            InstantiateSlots();
            UpdateSquadIcons();
        }

        void InstantiateSlots()
        {
            foreach (var unlockedMerc in App.Data.Mercs.UnlockedMercs)
            {
                MercManageSlot slot = Instantiate<MercManageSlot>(ManageMercSlot, null);

                slot.transform.SetParent(AvailableMercsParent, false);

                slot.Set(this, unlockedMerc.ID);

                Slots[unlockedMerc.ID] = slot;
            }
        }

        void CreateIcons()
        {
            for (int i = 0; i < GM.Common.Constants.MAX_SQUAD_SIZE; ++i)
            {
                MercManageIcon slot = Instantiate<MercManageIcon>(ManageMercIconObject, SquadIconsParent);

                Icons.Add(slot);
            }
        }

        void UpdateSquadIcons()
        {
            for (int i = 0; i < Icons.Count; i++)
            {
                MercManageIcon icon = Icons[i];

                if (SquadMercs.Count > i)
                {
                    MercManageSlot slot = SquadMercs[i];

                    icon.SetIcon(GetMercIconSprite(slot.Unit));
                }

                icon.gameObject.SetActive(SquadMercs.Count > i);
            }
        }

        Sprite GetMercIconSprite(UnitID unit) => App.Data.Mercs.GetGameMerc(unit).Icon;

        public void UpdateMerc(UnitID unit)
        {
            MercManageSlot slot = Slots[unit];

            UpdateSquadIcons();

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
