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
        private MercSquadController MercSquad;

        public bool SquadFull => SquadMercs.Count >= App.Mercs.MaxSquadSize;

        private readonly Dictionary<MercID, MercManageSlot> Slots = new Dictionary<MercID, MercManageSlot>();
        private readonly List<MercManageIcon> Icons = new List<MercManageIcon>();

        private List<MercManageSlot> SquadMercs => Slots.Values.Where(x => x.InSquad).OrderBy(x => x.MercID).ToList();

        private Action OnSavedChanges;

        public void AssignCallback(Action callback)
        {
            OnSavedChanges = callback;
        }

        private void Awake()
        {
            MercSquad = this.GetComponentInScene<MercSquadController>();

            CreateIcons();
            InstantiateSlots();
            UpdateSquadIcons();
        }

        private void Start()
        {
            ShowInnerPanel();
        }

        private void InstantiateSlots()
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

        private void CreateIcons()
        {
            for (int i = 0; i < App.Mercs.MaxSquadSize; ++i)
            {
                MercManageIcon slot = Instantiate<MercManageIcon>(ManageMercIconObject, SquadIconsParent);

                Icons.Add(slot);
            }
        }

        private void UpdateSquadIcons()
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

        private Sprite GetMercIconSprite(MercID unit)
        {
            return App.Mercs.GetMerc(unit).Icon;
        }

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
                MercSquad.RemoveUnitFromSquad(merc);
            }

            foreach (MercID merc in newSquadMercIds)
            {
                MercSquad.AddUnitToSquad(merc);
            }

            OnSavedChanges?.Invoke();

            Destroy(gameObject);
        }
    }
}
