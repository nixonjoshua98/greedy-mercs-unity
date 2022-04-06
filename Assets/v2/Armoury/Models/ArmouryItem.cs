using GM.Common.Enums;
using GMCommon.Enums;
using Newtonsoft.Json;
using UnityEngine;

namespace GM.Armoury.Models
{
    public class ArmouryItem
    {
        [JsonProperty(PropertyName = "ItemID")]
        public int ID;

        public BonusType BonusType;
        public ItemGrade Grade = ItemGrade.Common;

        public float BaseEffect;

        public float LevelEffect;

        [JsonIgnore]
        public string Name;

        [JsonIgnore]
        public Sprite Icon;
    }
}