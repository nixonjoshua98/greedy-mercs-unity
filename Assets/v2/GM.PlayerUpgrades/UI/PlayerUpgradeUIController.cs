using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM.PlayerUpgrades.UI
{
    public class PlayerUpgradeUIController : GM.UI.Panels.TogglablePanel
    {
        [Header("Upgrade Slots")]
        public GameObject MinorTapUpgradeSlot;

        [Header("References")]
        public GM.UI.AmountSelector UpgradeAmountSelector;
        [Space]
        public Transform UpgradeSlotParent;

        void Awake()
        {
            Instantiate<PlayerUpgradeUIObject>(MinorTapUpgradeSlot, UpgradeSlotParent).Init(UpgradeAmountSelector);
        }
    }
}