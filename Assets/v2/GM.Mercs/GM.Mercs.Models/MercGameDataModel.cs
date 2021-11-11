using Newtonsoft.Json;
using UnityEngine;
using MercID = GM.Common.Enums.MercID;

namespace GM.Mercs.Models
{
    public class MercGameDataModel
    {
        [JsonProperty(PropertyName = "mercId")]
        public MercID Id;

        [JsonProperty(PropertyName = "attackType")]
        public GM.Common.Enums.AttackType Attack;

        public double UnlockCost;
        public double BaseDamage = -1;

        public MercPassiveDataModel[] Passives;

        [JsonIgnore]
        public string Name;

        [JsonIgnore]
        public Sprite Icon;

        [JsonIgnore]
        public GameObject Prefab;
    }
}
