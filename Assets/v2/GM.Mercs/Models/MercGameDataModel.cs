using Newtonsoft.Json;
using UnityEngine;
using BonusType = GM.Common.Enums.BonusType;
using MercID = GM.Common.Enums.MercID;
using AttackType = GM.Common.Enums.AttackType;
namespace GM.Mercs.Models
{
    public class MercGameDataModel
    {
        [JsonProperty(PropertyName = "mercId")]
        public MercID Id;

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
