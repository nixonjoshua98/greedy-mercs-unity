using GM.Common.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Mercs
{
    public class StaticMercsModel
    {
        public List<MercPassiveBonus> Passives;
        public List<StaticMercData> Mercs;
    }

    public class MercPassiveBonus
    {
        [JsonProperty(PropertyName = "PassiveID")]
        public int ID;

        public BonusType BonusType;

        public float BonusValue;
    }

    public class MercPassive
    {
        public int PassiveID;

        public int UnlockLevel;

        /* Values set from MercPassiveBonus during setup */ 

        [JsonIgnore]
        public BonusType BonusType;

        [JsonIgnore]
        public float BonusValue;
    }

    public class StaticMercData
    {
        [JsonProperty(PropertyName = "MercID")]
        public MercID ID;

        public float BaseDamage;

        public List<MercPassive> Passives;

        public string Name = "Missing Merc Name";

        public UnitAttackType AttackType = UnitAttackType.Melee;

        public float RechargeRate;

        [JsonIgnore]
        public Sprite Icon;

        [JsonIgnore]
        public GameObject Prefab;
    }
}
