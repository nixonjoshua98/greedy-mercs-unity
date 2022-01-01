using UnityEngine;
using Newtonsoft.Json;
using GM.Common.Enums;

namespace GM.Armoury.Models
{
    public class ArmouryItemGameDataModel
    {
        [JsonProperty(PropertyName = "itemId", Required = Required.Always)]
        public int Id;

        [JsonProperty(Required = Required.Always)]
        public BonusType BonusType;

        [JsonProperty(Required = Required.Always)]
        public float BaseEffect;

        [JsonProperty(Required = Required.Always)]
        public float LevelEffect;

        [JsonIgnore]
        public string Name;

        [JsonIgnore]
        public Sprite Icon;
    }
}