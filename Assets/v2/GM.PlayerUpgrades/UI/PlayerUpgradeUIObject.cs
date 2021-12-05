using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GM.Common.Enums;
using GM.PlayerUpgrades.Data;
using GM.UI;

namespace GM.PlayerUpgrades.UI
{
    public abstract class PlayerUpgradeUIObject : SlotObject
    {
        protected abstract UpgradeID UpgradeId { get; set; }

        public PlayerUpgradeState Upgrade => App.Data.Upgrades.GetUpgradeState(UpgradeId);

        public virtual void Init(AmountSelector selector)
        {

        }
    }
}
