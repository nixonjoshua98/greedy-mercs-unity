using GM.Mercs.Data;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Mercs.UI
{
    public class MercsUIController : GM.Core.GMMonoBehaviour
    {
        [Header("Prefabs")]
        public GameObject SquadMercSlotObject;
        public GameObject ManageMercsObject;

        [Header("References")]
        public GM.UI.IntegerSelector QuantitySelector;
        public Transform SquadMercSlotsParent;

        private readonly Dictionary<MercID, MercUIObject> MercSlots = new();

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
            App.Mercs.E_OnMercUnlocked.AddListener((mercId) => UpdateSlotsUI());
        }

        private void UpdateSlotsUI()
        {
            foreach (var merc in App.Mercs.UnlockedMercs)
            {
                if (!MercSlots.ContainsKey(merc.MercID))
                {
                    InstantiateSlot(merc.MercID);
                }
            }
        }

        private void InstantiateSlot(MercID merc)
        {
            SquadMercSlot slot = this.Instantiate<SquadMercSlot>(SquadMercSlotObject, SquadMercSlotsParent);

            slot.Assign(merc, QuantitySelector);

            MercSlots.Add(merc, slot);

        }
    }
}