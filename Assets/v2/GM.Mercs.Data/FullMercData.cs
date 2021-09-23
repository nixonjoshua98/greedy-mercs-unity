using System.Linq;

namespace GM.Mercs.Data
{
    public struct FullMercData
    {
        public MercGameData GameData;
        public MercUserData User;

        public FullMercData(MercGameData gameData, MercUserData userData)
        {
            GameData = gameData;
            User = userData;
        }

        public MercID ID => GameData.ID;
        public BigDouble BaseDamage => StatsCache.BaseMercDamage(ID);
        public BigDouble CostToUpgrade(int levels) => Formulas.MercLevelUpCost(User.Level, levels, GameData.UnlockCost);

        public MercPassiveSkillData[] UnlockedPassives
        {
            get
            {
                int level = User.Level; // Need to reference it as a local

                return GameData.Passives.Where(ele => level >= ele.UnlockLevel).ToArray();
            }
        }
    }
}
