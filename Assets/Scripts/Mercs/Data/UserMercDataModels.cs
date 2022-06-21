using GM.Common.Enums;
using Newtonsoft.Json;

namespace GM.Mercs
{
    public class UserMercState
    {
        [JsonProperty(PropertyName = "MercID")]
        public MercID ID;
    }
}
