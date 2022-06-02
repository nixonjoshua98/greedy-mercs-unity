using GM.Common.Enums;
using Newtonsoft.Json;

namespace GM.Mercs
{
    public class UserMercDataModel
    {
        [JsonProperty(PropertyName = "mercId", Required = Required.Always)]
        public MercID ID;
    }
}
