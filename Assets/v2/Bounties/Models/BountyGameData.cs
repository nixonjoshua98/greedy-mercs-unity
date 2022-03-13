using Newtonsoft.Json;
using UnityEngine;

namespace GM.Bounties.Models
{
    public class BountyGameData
    {
        [JsonProperty(PropertyName = "bountyId")]
        [JsonRequired]
        public int Id;

        [JsonRequired]
        public int UnlockStage;

        [JsonRequired]
        public long HourlyIncome;

        [JsonIgnore]
        public string Name;

        [JsonIgnore]
        public Sprite Icon;

        [JsonIgnore]
        public GameObject Prefab;
    }
}