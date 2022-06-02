using GM.Common.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Mercs
{
    public class StaticMercsModel
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
        [JsonProperty(Required = Required.Always)]
        public int PassiveID { get; set; }

        [JsonProperty(Required = Required.Always)]
        public int UnlockLevel { get; set; }

        [JsonIgnore]
        public MercPassive Values { get; set; }
    }

    public class StaticMercData
    {
        [JsonProperty(PropertyName = "mercId", Required = Required.Always)]
        public MercID ID;

        [JsonProperty(Required = Required.Always)]
        public double BaseDamage;

        [JsonProperty(PropertyName = "passives")]
        public List<MercPassiveReference> Passives;

        [JsonProperty(PropertyName = "name")]
        public string Name = "Missing Merc Name";

        [JsonProperty(PropertyName = "attackType")]
        public UnitAttackType AttackType = UnitAttackType.Melee;

        [JsonProperty]
        public int SpawnEnergyRequired;

        [JsonIgnore]
        public Sprite Icon;

        [JsonIgnore]
        public GameObject Prefab;
    }
}
