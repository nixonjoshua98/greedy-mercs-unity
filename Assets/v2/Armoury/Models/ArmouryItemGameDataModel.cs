using UnityEngine;
using Newtonsoft.Json;

namespace GM.Armoury.Models
{
    public class ArmouryItemGameDataModel
    {
        [JsonProperty(PropertyName = "itemId")]
        [JsonRequired]
        public int Id;

        [JsonProperty(PropertyName = "itemTier")]
        [JsonRequired]
        public int Tier;

        [JsonProperty(PropertyName = "baseDamageMultiplier")]
        [JsonRequired]
        public float BaseDamage;

        [JsonIgnore]
        public string Name;

        [JsonIgnore]
        public Sprite Icon;

        [JsonRequired]
        public int MaxMergeLevel;

        [JsonRequired]
        public int BaseMergeCost;

        [JsonIgnore]
        public ItemTierDisplayConfig Config => ArmouryUtils.GetDisplayConfig(Tier);
    }
}