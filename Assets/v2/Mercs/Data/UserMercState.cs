using GM.Common.Enums;
using Newtonsoft.Json;

namespace GM.Mercs.Data
{
    public class UserMercState
    {
        [JsonProperty(Required = Required.Always)]
        public readonly UnitID ID;

        public int Level = 1;

        public bool InSquad = false;

        private UserMercState() { }

        public UserMercState(UnitID unit)
        {
            ID = unit;
        }
    }
}
