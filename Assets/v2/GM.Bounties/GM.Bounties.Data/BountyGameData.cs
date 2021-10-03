using UnityEngine;

namespace GM.Bounties.Data
{
    /// <summary>
    /// Combined bounty game data struct for BountyLocalGameData and server data
    /// </summary>
    public class BountyGameData
    {
        public int ID;
        public string Name;

        public int UnlockStage;
        public int HourlyIncome;

        public float SpawnChance;

        public Sprite Icon;
        public GameObject Prefab;
        public GM.Bounties.UI.BountySlot Slot;
    }
}