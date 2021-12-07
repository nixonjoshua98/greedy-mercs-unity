using GM.Upgrades.Data;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GM.Upgrades.UI
{
    public class UpgradesUIController : GM.UI.Panels.TogglablePanel
    {
        [Header("Upgrade Slots")]
        public GameObject MinorTapUpgradeSlot;
        public GameObject MajorTapUpgradeSlot;

        [Header("References")]
        public GM.UI.AmountSelector UpgradeAmountSelector;
        [Space]
        public Transform UpgradeSlotParent;

        Dictionary<UpgradeID, IUpgradeSlot> UpgradeSlots = new Dictionary<UpgradeID, IUpgradeSlot>();

        void Awake()
        {
            UpgradeSlots[UpgradeID.MINOR_TAP] = Instantiate<UpgradeSlot<UpgradeState>>(MinorTapUpgradeSlot, UpgradeSlotParent);
            UpgradeSlots[UpgradeID.MAJOR_TAP] = Instantiate<UpgradeSlot<UpgradeState>>(MajorTapUpgradeSlot, UpgradeSlotParent);

            UpgradeSlots.Values.ToList().ForEach(slot => slot.Init(UpgradeAmountSelector));
        }
    }
}