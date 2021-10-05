using Newtonsoft.Json;

namespace GM.Armoury.Models
{
    public class UserArmouryItemModel
    {
        [JsonProperty(PropertyName = "itemId")]
        public int Id;

        public int Level;

        [JsonProperty(PropertyName = "owned")]
        public int NumOwned;

        public int EvoLevel;
    }
}
