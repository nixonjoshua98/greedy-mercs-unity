using UnityEngine;
using Newtonsoft.Json;

namespace GM.Armoury.Models
{
    public class ArmouryItemGameDataModel
    {
        [JsonProperty(PropertyName = "itemId")]
        public int Id;

        [JsonProperty(PropertyName = "itemTier")]
        public int Tier;

        [JsonProperty(PropertyName = "baseDamageMultiplier")]
        public float BaseDamage;

        [JsonIgnore]
        public string Name;

        [JsonIgnore]
        public Sprite Icon;

        [JsonIgnore]
        public int MaxEvolveLevel;

        [JsonIgnore]
        public int EvoLevelCost;
    }
}