using UnityEngine;
using Newtonsoft.Json;

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
        public int HourlyIncome;

        [JsonIgnore]
        public float SpawnChance = 1.0f;

        [JsonIgnore]
        public string Name;

        [JsonIgnore]
        public Sprite Icon;

        [JsonIgnore]
        public GameObject Prefab;
    }
}