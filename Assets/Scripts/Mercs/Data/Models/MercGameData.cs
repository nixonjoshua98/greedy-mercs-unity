using Newtonsoft.Json;
using SRC.Common.Enums;
using System.Collections.Generic;

namespace SRC.Mercs.Data
{
    public class MercDataFile
    {
        public List<MercPassiveBonus> Passives;
        public List<MercGameData> Mercs;
    }

    public class MercPassiveBonus
    {
        public int PassiveID;

        public BonusType BonusType;

        public float BonusValue;
    }

    public class MercPassive
    {
        public int PassiveID;

        public int UnlockLevel;

        [JsonIgnore]
        public BonusType BonusType;

        [JsonIgnore]
        public float BonusValue;
    }

    public class MercGameData
    {
        public MercID MercID;

        public float BaseDamage;

        public List<MercPassive> Passives;

        public string Name = "Missing Merc Name";

        public UnitAttackType AttackType = UnitAttackType.Melee;

        public float RechargeRate;

        public Rarity Grade;
    }
}
