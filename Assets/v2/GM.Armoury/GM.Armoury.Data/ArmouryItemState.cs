using Newtonsoft.Json;

namespace GM.Armoury.Data
{
    /// <summary>
    /// User state of the armoury item
    /// </summary>
    public class ArmouryItemState
    {
        [JsonProperty(PropertyName = "itemId")]
        public int Id;

        public int Level;

        [JsonProperty(PropertyName = "owned")]
        public int NumOwned;

        public int EvoLevel;
    }
}
