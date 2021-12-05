using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GM.Common.Enums;

namespace GM.PlayerUpgrades.Data
{
    public class PlayerUpgrades
    {
        Dictionary<UpgradeID, PlayerUpgradeState> Upgrades;

        public PlayerUpgrades()
        {
            Upgrades = new Dictionary<UpgradeID, PlayerUpgradeState>()
            {
                { UpgradeID.MINOR_TAP_DAMAGE, new PlayerUpgradeState(1, 1_000) }
            };
        }

        public PlayerUpgradeState GetUpgradeState(UpgradeID upgradeId)
        {
            if (!Upgrades.TryGetValue(upgradeId, out PlayerUpgradeState state))
            {
                Debug.LogError($"Upgrade {upgradeId} was not found");
            }

            return state;
        }
    }
}
