using Newtonsoft.Json;
using UnityEngine;
using GM.Common.Enums;

namespace GM.Mercs.Models
{
    public struct MercPassiveDataModel
    {
        [JsonProperty(PropertyName = "bonusType")]
        public BonusType Type;

        [JsonProperty(PropertyName = "bonusValue")]
        public float Value;

        public int UnlockLevel;
    }

    public class MercGameDataModel
    {
        [JsonProperty(PropertyName = "mercId", Required = Required.Always)]
        public MercID Id;

        [JsonProperty(PropertyName = "isDefault", Required = Required.Always)]
        public bool IsDefault;

        [JsonProperty(Required = Required.Always)]
        public double BaseUpgradeCost;

        public double BaseDamage = -1;

        public MercPassiveDataModel[] Passives;

        [JsonIgnore]
        public AttackType AttackType;

        [JsonIgnore]
        public string Name;

        [JsonIgnore]
        public Sprite Icon;

        [JsonIgnore]
        public GameObject Prefab;
    }
}
