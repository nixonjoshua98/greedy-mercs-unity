using GM.Common.Enums;
using Newtonsoft.Json;

namespace GM.Mercs.Data
{
    public class UserMercLocalState
    {
        [JsonProperty]
        public MercID ID;

        [JsonProperty]
        public int Level = 1;

        [JsonProperty]
        public float RechargeProgress = 0.0f;

        private UserMercLocalState() { }

        public UserMercLocalState(MercID unit)
        {
            ID = unit;
        }
    }
}
