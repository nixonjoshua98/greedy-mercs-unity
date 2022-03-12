using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Upgrades
{
    public interface IUpgradeRequirement
    {
        bool IsUnlocked { get; }
    }

    public class DefaultUpgradeRequirement : IUpgradeRequirement
    {
        public static DefaultUpgradeRequirement Value = new DefaultUpgradeRequirement();

        public bool IsUnlocked => true;
    }


    public class UpgradeGoldRequirement : Core.GMClass, IUpgradeRequirement
    {
        BigDouble GoldRequired;

        bool HasHitGoldTarget = false;

        public UpgradeGoldRequirement(BigDouble gold)
        {
            GoldRequired = gold;
        }

        public bool IsUnlocked
        {
            get
            {
                HasHitGoldTarget = HasHitGoldTarget || App.Inventory.Gold >= GoldRequired;

                return HasHitGoldTarget;
            }
        }
    }
}
