using GM.Common.Enums;
using Newtonsoft.Json;

namespace GM.Mercs.Models
{
    public class UserMercDataModel
    {
        [JsonProperty(PropertyName = "unitId", Required = Required.Always)]
        public UnitID Id;
    }
}
