using UnityEngine;

namespace GM.Artefacts.Data
{
    /// <summary>
    /// Combined local and server artefact game data
    /// </summary>
    public struct ArtefactGameData
    {
        public int ID;

        public string Name;

        public BonusType Bonus;

        public int MaxLevel;

        // Level + effect values
        public float CostExpo;
        public float CostCoeff;
        public float BaseEffect;
        public float LevelEffect;

        // Unity objects
        public Sprite Icon;
        public GM.Artefacts.UI.ArtefactSlot Slot;
    }
}
