using Newtonsoft.Json;

namespace GM.Armoury.Models
{
    public class ArmouryItemUserDataModel
    {
        [JsonProperty(PropertyName = "itemId")]
        [JsonRequired]
        public int Id;

        [JsonProperty(PropertyName = "owned")]
        public int NumOwned = 0;

        [JsonProperty]
        public int MergeLevel = 0;

        [JsonProperty]
        public int Level = 0;
    }
}
