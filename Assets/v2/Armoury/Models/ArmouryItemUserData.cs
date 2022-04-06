using Newtonsoft.Json;

namespace GM.Armoury.Models
{
    public class ArmouryItemUserData
    {
        [JsonProperty(PropertyName = "ItemID")]
        public int Id;

        [JsonProperty(PropertyName = "Owned")]
        public int NumOwned = 0;

        public int MergeLevel = 0;

        public int Level = 0;
    }
}
