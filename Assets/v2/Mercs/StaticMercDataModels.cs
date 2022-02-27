using GM.Common.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Mercs
{
    public class StaticMercsDataResponse
    {
        public List<MercPassive> Passives;
        public List<StaticMercData> Mercs;
    }

    public class MercPassive
    {
        [JsonProperty(PropertyName = "passiveId")]
        public int ID;

        [JsonProperty(PropertyName = "bonusType")]
        public BonusType Type;

        [JsonProperty(PropertyName = "bonusValue")]
        public float Value;
    }

    public class MercPassiveReference
    {
        public int PassiveID { get; set; } = 0;

        [JsonProperty(Required = Required.Always)]
        public int UnlockLevel { get; set; }

        [JsonIgnore]
        public MercPassive Values { get; set; }
    }

    public class StaticMercData
    {
        [JsonProperty(PropertyName = "mercId", Required = Required.Always)]
        public UnitID ID;

        [JsonProperty]
        public bool IsDefault = false;

        [JsonProperty(Required = Required.Always)]
        public double BaseUpgradeCost;

        [JsonProperty(Required = Required.Always)]
        public double BaseDamage;

        [JsonProperty(PropertyName = "passives")]
        public List<MercPassiveReference> Passives;

        [JsonProperty(PropertyName = "name")]
        public string Name = "Missing Merc Name";

        [JsonProperty(PropertyName = "attackType")]
        public AttackType AttackType = AttackType.MELEE; 

        [JsonIgnore]
        public Sprite Icon;

        [JsonIgnore]
        public GameObject Prefab;
    }
}
