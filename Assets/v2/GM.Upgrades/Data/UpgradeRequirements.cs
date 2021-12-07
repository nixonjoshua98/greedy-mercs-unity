using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Upgrades
{
    public interface IUpgradeUnlockRequire
    {
        public bool IsDefault { get; }
        bool IsUnlocked { get; }
    }

    public class DefaultUpgrade : IUpgradeUnlockRequire
    {
        public static DefaultUpgrade Value = new DefaultUpgrade();

        public bool IsDefault => true;
        public bool IsUnlocked => true;
    }


    public class UpgradeGoldRequirement : Core.GMClass, IUpgradeUnlockRequire
    {
        public bool IsDefault => false;

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
                HasHitGoldTarget = HasHitGoldTarget || App.Data.Inv.Gold >= GoldRequired;

                return HasHitGoldTarget;
            }
        }
    }
}
