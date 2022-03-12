using GM.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GM.Mercs.UI
{
    public class MercManagePopup : GM.UI.PopupPanelBase
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

            CreateIcons();
            InstantiateSlots();
            UpdateSquadIcons();
        }

        void Start()
        {
            ShowInnerPanel();
        }

        void InstantiateSlots()
        {
            foreach (var unlockedMerc in App.GMData.Mercs.UnlockedMercs)
            {
                MercManageSlot slot = Instantiate<MercManageSlot>(ManageMercSlot, null);

                slot.transform.SetParent(AvailableMercsParent, false);

                slot.Set(this, unlockedMerc.ID);

                Slots[unlockedMerc.ID] = slot;
            }

            Slots.Values.ToList().ForEach(slot => slot.UpdateActiveUI());
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

        Sprite GetMercIconSprite(UnitID unit) => App.GMData.Mercs.GetGameMerc(unit).Icon;

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

            foreach (UnitID merc in App.GMData.Mercs.MercsInSquad)
            {
                MercSquad.RemoveFromSquad(merc);
            }

            foreach (UnitID merc in newSquadMercIds)
            {
                MercSquad.AddToQueue(merc);
            }

            OnSavedChanges?.Invoke();

            Destroy(gameObject);
        }
    }
}
