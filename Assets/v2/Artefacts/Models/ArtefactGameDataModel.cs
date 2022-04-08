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

        public string Name;

        public float CostExpo;

        public float CostCoeff;

        public float BaseEffect;

        public float LevelEffect;

        [JsonIgnore]
        public Sprite Icon;

        [JsonIgnore]
        public Sprite IconBackground;
    }
}