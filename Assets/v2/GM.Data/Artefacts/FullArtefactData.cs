namespace GM.Data.Artefacts
{
    public struct FullArtefactData
    {
        public GM.Artefacts.ArtefactData Values;
        public ArtefactState State;

        public FullArtefactData(GM.Artefacts.ArtefactData values, ArtefactState state)
        {
            Values = values;
            State = state;
        }


        // === Properties === //
        public int ID => Values.ID;
        public double BaseEffect => Formulas.BaseArtefactEffect(State.Level, Values.BaseEffect, Values.LevelEffect);
        public bool IsMaxLevel => State.Level >= Values.MaxLevel;

        // === Methods === //
        public System.Numerics.BigInteger CostToUpgrade(int levels) => Formulas.ArtefactLevelUpCost(State.Level, levels, Values.CostExpo, Values.CostCoeff);
    }
}
