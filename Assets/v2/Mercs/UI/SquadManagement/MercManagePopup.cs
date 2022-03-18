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

        public bool SquadFull => SquadMercs.Count >= App.Mercs.MaxSquadSize;

        Dictionary<MercID, MercManageSlot> Slots = new Dictionary<MercID, MercManageSlot>();
        List<MercManageIcon> Icons = new List<MercManageIcon>();
        List<MercManageSlot> SquadMercs => Slots.Values.Where(x => x.InSquad).OrderBy(x => x.MercID).ToList();


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
            foreach (var unlockedMerc in App.Mercs.UnlockedMercs)
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
            for (int i = 0; i < App.Mercs.MaxSquadSize; ++i)
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

                    icon.SetIcon(GetMercIconSprite(slot.MercID));
                }

                icon.gameObject.SetActive(SquadMercs.Count > i);
            }
        }

        Sprite GetMercIconSprite(MercID unit) => App.Mercs.GetGameMerc(unit).Icon;

        public void UpdateMerc(MercID unit)
        {
            MercManageSlot slot = Slots[unit];

            UpdateSquadIcons();

            Slots.Values.ToList().ForEach(x => x.UpdateActiveUI());
        }

        // = UI Callbacks = //

        public void Button_SaveChanges()
        {
            List<MercID> newSquadMercIds = Slots.Where(x => x.Value.InSquad).Select(x => x.Key).ToList();

            foreach (MercID merc in App.Mercs.MercsInSquad)
            {
                MercSquad.RemoveFromSquad(merc);
            }

            foreach (MercID merc in newSquadMercIds)
            {
                MercSquad.AddToSquad(merc);
            }

            OnSavedChanges?.Invoke();

            Destroy(gameObject);
        }
    }
}
