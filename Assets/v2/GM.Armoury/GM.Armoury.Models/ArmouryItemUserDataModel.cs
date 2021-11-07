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

        public int MergeLevel = 0;

        [JsonProperty(PropertyName = "level")]
        public int ActualLevel = 0;
        public int DummyLevel;
        public int Level => ActualLevel + DummyLevel;
    }
}
