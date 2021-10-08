using UnityEngine;
using Newtonsoft.Json;

namespace GM.Bounties.Models
{
    public class BountyGameData
    {
        [JsonProperty(PropertyName = "bountyId")]
        public int ID;

        public int UnlockStage;
        public int HourlyIncome;

        public float SpawnChance = 1.0f;

        [JsonIgnore]
        public string Name;

        [JsonIgnore]
        public Sprite Icon;

        [JsonIgnore]
        public GameObject Prefab;

        [JsonIgnore]
        public UI.BountySlot Slot;
    }
}