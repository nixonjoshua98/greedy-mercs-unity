using GM.Common.Enums;
using Newtonsoft.Json;

namespace GM.Mercs
{
    public class UserMercDataModel
    {
        [JsonProperty(PropertyName = "unitId", Required = Required.Always)]
        public UnitID ID;
    }
}
