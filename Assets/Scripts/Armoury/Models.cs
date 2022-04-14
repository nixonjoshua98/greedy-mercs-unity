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

        public ItemGrade Grade;

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
        public int NumOwned;

        public int Level;
    }
}
