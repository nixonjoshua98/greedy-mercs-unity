using GM.Enums;
using Newtonsoft.Json;
using UnityEngine;

namespace GM.Artefacts.Models
{
    public class Artefact
    {
        [JsonProperty(PropertyName = "ArtefactID")]
        public int ID;

        [JsonProperty(PropertyName = "BonusType")]
        public BonusType Bonus;

        public ItemGrade GradeID;

        public int MaxLevel = 1_000;
        public string Name;
        public float CostExpo;
        public float CostCoeff;
        public float BaseEffect;
        public float LevelEffect;

        [JsonIgnore]
        public Sprite Icon;
    }
}