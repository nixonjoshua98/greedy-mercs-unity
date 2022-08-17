using Newtonsoft.Json;
using SRC.Common.Enums;
using UnityEngine;

namespace SRC.Armoury.Data
{
    public class ArmouryItem
    {
        [JsonProperty(PropertyName = "ItemID")]
        public int ID;

        public BonusType BonusType;

        public Rarity Grade;

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
