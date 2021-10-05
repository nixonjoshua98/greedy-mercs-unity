using GM.Artefacts.Models;

namespace GM.Artefacts.Data
{
    public struct FullArtefactData
    {
        public ArtefactGameData Game;
        public UserArtefactModel User;

        public FullArtefactData(ArtefactGameData values, UserArtefactModel state)
        {
            Game = values;
            User = state;
        }

        // === Properties === //
        public int ID => Game.Id;
        public double BaseEffect => Formulas.BaseArtefactEffect(User.Level, Game.BaseEffect, Game.LevelEffect);
        public bool IsMaxLevel => User.Level >= Game.MaxLevel;

        // === Methods === //
        public System.Numerics.BigInteger CostToUpgrade(int levels)
        {
            return Formulas.ArtefactLevelUpCost(User.Level, levels, Game.CostExpo, Game.CostCoeff);
        }
    }
}
