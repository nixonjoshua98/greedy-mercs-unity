using Newtonsoft.Json;

namespace SRC.Mercs.Data
{
    public class MercUserData
    {
        [JsonProperty(PropertyName = "MercID")]
        public MercID ID;
    }
}
