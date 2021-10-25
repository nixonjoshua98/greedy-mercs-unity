using System.Linq;
using UnityEngine;

namespace GM.Mercs.Data
{
    public struct FullMercData
    {
        Models.MercGameDataModel Game;
        MercUserData User;

        public FullMercData(Models.MercGameDataModel gameData, MercUserData userData)
        {
            Game = gameData;
            User = userData;
        }

        // == User == //
        public int CurrentLevel { get { return User.Level; } set { User.Level = value; } }

        public void LevelUp(int level)
        {
            User.Level += level;
        }

        // == Game == //
        public string Name => Game.Name;
        public Sprite Icon => Game.Icon;
        public double UnlockCost => Game.UnlockCost;
        public Common.Enums.AttackType Attack => Game.Attack;
        public Models.MercPassiveDataModel[] Passives => Game.Passives;
        public BigDouble BaseDamage => Game.BaseDamage;
        public BigDouble BaseDamageFor => StatsCache.BaseMercDamage(Game.Id);
        public BigDouble CostToUpgrade(int levels) => Formulas.MercLevelUpCost(User.Level, levels, Game.UnlockCost);
        public Models.MercPassiveDataModel[] UnlockedPassives
        {
            get
            {
                var temp = this; // Linq weirdness

                return Game.Passives.Where(p => temp.IsPassiveUnlocked(p)).ToArray();
            }
        }

        // === Private Methods === //
        bool IsPassiveUnlocked(Models.MercPassiveDataModel passive) => User.Level >= passive.UnlockLevel;
    }
}