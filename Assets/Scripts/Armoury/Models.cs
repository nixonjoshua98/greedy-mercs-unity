using GM.Common.Enums;
using GMCommon.Enums;
using Newtonsoft.Json;
using UnityEngine;

namespace GM.Armoury.Data
{
    public class ArmouryItem
    {
        [JsonProperty(PropertyName = "ItemID")]
        public int ID;

        public BonusType BonusType;
        public ItemGrade Grade = ItemGrade.Common;

        public float BaseEffect;

        public float LevelEffect;

        public string Name = "No Name";

        [JsonIgnore]
        public Sprite Icon;
    }

    public class UserArmouryItem
    {
        [JsonProperty(PropertyName = "ItemID")]
        public int ID;

        [JsonProperty(PropertyName = "Owned")]
        public int NumOwned = 0;

        public int MergeLevel = 0;

        public int Level = 0;
    }
}
