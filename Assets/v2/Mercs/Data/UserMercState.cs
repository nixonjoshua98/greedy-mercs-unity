using GM.Common.Enums;
using Newtonsoft.Json;

namespace GM.Mercs.Data
{
    public class UserMercState : GM.Core.GMClass
    {
        [JsonProperty(Required = Required.Always)]
        public readonly UnitID ID;

        [JsonProperty]
        public int Level = 1;

        [JsonIgnore]
        public bool InSquad { get => App.PersistantLocalFile.SquadMercIDs.Contains(ID); }

        [JsonIgnore]
        public float CurrentSpawnEnergy = 0.0f;

        private UserMercState() { }

        public UserMercState(UnitID unit)
        {
            ID = unit;
        }
    }
}
