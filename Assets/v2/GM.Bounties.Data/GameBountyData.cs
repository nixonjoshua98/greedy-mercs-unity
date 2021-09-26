using UnityEngine;

namespace GM.Bounties.Data
{
    public struct GameBountyData
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