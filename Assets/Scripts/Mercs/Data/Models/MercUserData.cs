using GM.Common.Enums;
using Newtonsoft.Json;

namespace GM.Mercs.Data
{
    public class MercUserData
    {
        [JsonProperty(PropertyName = "MercID")]
        public MercID ID;
    }
}
