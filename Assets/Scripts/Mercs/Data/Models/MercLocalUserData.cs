using Newtonsoft.Json;

namespace GM.Mercs.Data
{
    public class MercLocalUserData
    {
        [JsonProperty]
        public MercID ID;

        [JsonProperty]
        public int Level = 1;

        private MercLocalUserData() { }

        public MercLocalUserData(MercID unit)
        {
            ID = unit;
        }
    }
}
