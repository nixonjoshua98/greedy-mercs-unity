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
        List<UpgradeID> UnlockedUpgrades = new List<UpgradeID>();

        void Awake()
        {
            UpgradeSlots[UpgradeID.MINOR_TAP] = Instantiate<UpgradeSlot<UpgradeState>>(MinorTapUpgradeSlot, UpgradeSlotParent);
            UpgradeSlots[UpgradeID.MAJOR_TAP] = Instantiate<UpgradeSlot<UpgradeState>>(MajorTapUpgradeSlot, UpgradeSlotParent);

            UpgradeSlots.Values.ToList().ForEach(slot => slot.Init(UpgradeAmountSelector));
        }

        void Start()
        {
            UpdateSlots();
        }

        void UpdateSlots()
        {
            foreach ((UpgradeID upgrade, IUpgradeSlot slot) in UpgradeSlots.Select(x => (x.Key, x.Value)))
            {
                UpgradeState state = App.Data.Upgrades.GetUpgrade(upgrade);

                if (!UnlockedUpgrades.Contains(upgrade))
                {
                    if (state.UnlockRequirement.IsUnlocked)
                    {
                        UnlockedUpgrades.Add(upgrade);

                        if (!state.UnlockRequirement.IsDefault)
                        {
                            UpgradeSlots[upgrade].OnUnlocked();
                        }
                    }
                    else
                    {
                        slot.Hide();
                    }
                }
            }
        }

        void FixedUpdate()
        {
            UpdateSlots();
        }
    }
}