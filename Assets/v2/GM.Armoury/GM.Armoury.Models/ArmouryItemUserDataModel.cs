using Newtonsoft.Json;

namespace GM.Armoury.Models
{
    public class ArmouryItemUserDataModel
    {
        [JsonProperty(PropertyName = "itemId")]
        public int Id;

        public int Level = 0;

        [JsonProperty(PropertyName = "owned")]
        public int NumOwned = 0;

        public int EvoLevel = 0;
    }
}
