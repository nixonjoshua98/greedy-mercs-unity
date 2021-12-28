using Newtonsoft.Json;
using UnityEngine;
using AttackType = GM.Common.Enums.AttackType;
using MercID = GM.Common.Enums.MercID;

namespace GM.Mercs.Models
{
    public class MercGameDataModel
    {
        [JsonProperty(PropertyName = "mercId", Required = Required.Always)]
        public MercID Id;

        [JsonProperty(PropertyName = "isDefault", Required = Required.Always)]
        public bool IsDefault;

        public double UnlockCost;
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
