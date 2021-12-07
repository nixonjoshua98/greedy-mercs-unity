using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Upgrades
{
    public interface IUpgradeRequirement
    {
        public bool IsDefault { get; }
        bool IsUnlocked { get; }
    }

    public class DefaultUpgradeRequirement : IUpgradeRequirement
    {
        public static DefaultUpgradeRequirement Value = new DefaultUpgradeRequirement();

        public bool IsDefault => true;
        public bool IsUnlocked => true;
    }


    public class UpgradeGoldRequirement : Core.GMClass, IUpgradeRequirement
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
