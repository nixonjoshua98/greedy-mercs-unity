using System.Linq;

namespace GM.Mercs.Data
{
    public struct FullMercData
    {
        public MercGameData Game;
        public MercUserData User;

        public FullMercData(MercGameData gameData, MercUserData userData)
        {
            Game = gameData;
            User = userData;
        }

        public MercID Id => Game.Id;
        public BigDouble BaseDamage => StatsCache.BaseMercDamage(Game.Id);
        public BigDouble CostToUpgrade(int levels) => Formulas.MercLevelUpCost(User.Level, levels, Game.UnlockCost);
        public MercPassiveSkillData[] UnlockedPassives
        {
            get
            {
                var temp = this; // Linq weirdness

                return Game.Passives.Where(p => temp.IsPassiveUnlocked(p)).ToArray();
            }
        }

        // === Private Methods === //
        bool IsPassiveUnlocked(MercPassiveSkillData passive) => User.Level >= passive.UnlockLevel;
    }
}