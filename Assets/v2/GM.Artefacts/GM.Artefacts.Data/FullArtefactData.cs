namespace GM.Artefacts.Data
{
    /// <summary>
    /// Aggregated class for both artefact game and user data
    /// </summary>
    public struct FullArtefactData
    {
        public ArtefactGameData Game;
        public ArtefactState User;

        public FullArtefactData(ArtefactGameData values, ArtefactState state)
        {
            Game = values;
            User = state;
        }

        // === Properties === //
        public int ID => Game.ID;
        public double BaseEffect => Formulas.BaseArtefactEffect(User.Level, Game.BaseEffect, Game.LevelEffect);
        public bool IsMaxLevel => User.Level >= Game.MaxLevel;

        // === Methods === //
        public System.Numerics.BigInteger CostToUpgrade(int levels)
        {
            return Formulas.ArtefactLevelUpCost(User.Level, levels, Game.CostExpo, Game.CostCoeff);
        }
    }
}
