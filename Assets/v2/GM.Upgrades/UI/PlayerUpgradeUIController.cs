using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GM.Upgrades.Data;

namespace GM.Upgrades.UI
{
    public class PlayerUpgradeUIController : GM.UI.Panels.TogglablePanel
    {
        [Header("Upgrade Slots")]
        public GameObject MinorTapUpgradeSlot;
        public GameObject MajorTapUpgradeSlot;

        [Header("References")]
        public GM.UI.AmountSelector UpgradeAmountSelector;
        [Space]
        public Transform UpgradeSlotParent;

        void Awake()
        {
            Instantiate<UpgradeSlot<UpgradeState>>(MinorTapUpgradeSlot, UpgradeSlotParent).Init(UpgradeAmountSelector);
            Instantiate<UpgradeSlot<UpgradeState>>(MajorTapUpgradeSlot, UpgradeSlotParent).Init(UpgradeAmountSelector);
        }
    }
}