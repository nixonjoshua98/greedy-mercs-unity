using GM.Common.Enums;
using Newtonsoft.Json;

namespace GM.Mercs.Data
{
    public class UserMercState : GM.Core.GMClass
    {
        [JsonProperty(Required = Required.Always)]
        public readonly MercID ID;

        [JsonProperty]
        public int Level = 1;

        [JsonProperty]
        public float CurrentSpawnEnergy = 0.0f;

        private UserMercState() { }

        public UserMercState(MercID unit)
        {
            ID = unit;
        }
    }
}
