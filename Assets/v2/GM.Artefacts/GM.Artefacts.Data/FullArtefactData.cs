namespace GM.Artefacts.Data
{
    /// <summary>
    /// Aggregated class for both artefact game and user data
    /// </summary>
    public struct FullArtefactData
    {
        public ArtefactGameData Values;
        public ArtefactState State;

        public FullArtefactData(ArtefactGameData values, ArtefactState state)
        {
            Values = values;
            State = state;
        }


        // === Properties === //
        public int ID => Values.ID;
        public double BaseEffect => Formulas.BaseArtefactEffect(State.Level, Values.BaseEffect, Values.LevelEffect);
        public bool IsMaxLevel => State.Level >= Values.MaxLevel;

        // === Methods === //
        public System.Numerics.BigInteger CostToUpgrade(int levels)
        {
            return Formulas.ArtefactLevelUpCost(State.Level, levels, Values.CostExpo, Values.CostCoeff);
        }
    }
}
