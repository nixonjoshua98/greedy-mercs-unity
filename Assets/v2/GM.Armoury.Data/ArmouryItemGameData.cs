using UnityEngine;

namespace GM.Armoury.Data
{
    public struct ArmouryItemGameData
    {
        public int ID;

        public int Tier;

        public string Name;

        public float BaseDamage;

        // Unity Objects
        public Sprite Icon;

        // Evolve Values
        public int MaxEvolveLevel;
        public int EvoLevelCost;
    }
}