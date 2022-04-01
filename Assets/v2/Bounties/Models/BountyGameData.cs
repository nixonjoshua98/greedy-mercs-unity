using Newtonsoft.Json;
using UnityEngine;

namespace GM.Bounties.Models
{
    public class BountyGameData
    {
        [JsonProperty(PropertyName = "BountyID")]
        public int ID;

        public int UnlockStage;

        public long HourlyIncome;

        public string Name;

        [JsonIgnore]
        public Sprite Icon;

        [JsonIgnore]
        public GameObject Prefab;
    }
}