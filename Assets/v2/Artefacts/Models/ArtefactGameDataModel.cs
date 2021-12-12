using UnityEngine;
using Newtonsoft.Json;
using BonusType = GM.Common.Enums.BonusType;
using GM.Artefacts.Data;

namespace GM.Artefacts.Models
{
    public class ArtefactGameDataModel
    {
        [JsonProperty(PropertyName = "artefactId")]
        public int Id;

        [JsonProperty(PropertyName = "bonusType")]
        public BonusType Bonus;

        public int MaxLevel = 1_000;

        public float CostExpo;
        public float CostCoeff;
        public float BaseEffect;
        public float LevelEffect;

        [JsonIgnore]
        public string Name;

        [JsonIgnore]
        public Sprite Icon;

        [JsonIgnore]
        public Sprite IconBackground;

        [JsonIgnore]
        public ArtefactDisplayConfig DisplayConfig;
    }
}
