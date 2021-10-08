using UnityEngine;
using Newtonsoft.Json;

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
        public UI.ArtefactSlot Slot;
    }
}
