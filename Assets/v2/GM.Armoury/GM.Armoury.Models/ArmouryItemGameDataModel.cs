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

        public int MaxStarLevel;

        public int BaseStarLevelCost;

        [JsonIgnore]
        public ItemTierDisplayConfig Config => ArmouryUtils.GetDisplayConfig(Tier);
    }
}