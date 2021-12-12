using Newtonsoft.Json;
using UnityEngine;
using BonusType = GM.Common.Enums.BonusType;

namespace GM.Artefacts.Models
{
    public class ArtefactGameDataModel
    {
        [JsonProperty(PropertyName = "artefactId")]
        public int Id;

        [JsonProperty(PropertyName = "bonusType")]
        public BonusType Bonus;

        public int MaxLevel = 1_000;

        [JsonRequired]
        public float CostExpo;

        [JsonRequired]
        public float CostCoeff;

        [JsonRequired]
        public float BaseEffect;

        [JsonRequired]
        public float LevelEffect;

        [JsonIgnore]
        public string Name;

        [JsonIgnore]
        public Sprite Icon;

        [JsonIgnore]
        public Sprite IconBackground;
    }
}