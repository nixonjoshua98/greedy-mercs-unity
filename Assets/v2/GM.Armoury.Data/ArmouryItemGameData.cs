using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Armoury.Data
{
    public struct ArmouryItemData
    {
        public int ID;

        public int Tier;

        public string Name;

        public float BaseDamageMultiplier;

        public Sprite Icon;

        // Evolve Values
        public int MaxEvolveLevel;
        public int EvoLevelCost;
    }
}