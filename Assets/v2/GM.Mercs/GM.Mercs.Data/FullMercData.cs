using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace GM.Mercs.Data
{
    public class FullMercData
    {
        Models.MercGameDataModel Game;
        MercUserData User;

        public FullMercData(Models.MercGameDataModel gameData, MercUserData userData)
        {
            Game = gameData;
            User = userData;
        }

        public void IncrementLevel(int level)
        {
            User.Level += level;
        }

        public MercID Id => Game.Id;
        public string Name => Game.Name;
        public int CurrentLevel => User.Level;
        public bool IsMaxLevel => User.Level >= GM.Constants.MAX_MERC_LEVEL;
        public Sprite Icon => Game.Icon;
        public double UnlockCost => Game.UnlockCost;
        public Common.Enums.AttackType Attack => Game.Attack;
        public Models.MercPassiveDataModel[] Passives => Game.Passives;
        public BigDouble BaseDamage => Game.BaseDamage;
        public List<Models.MercPassiveDataModel> UnlockedPassives
        {
            get
            {
                var temp = this; // Linq weirdness

                return Game.Passives.Where(p => temp.IsPassiveUnlocked(p)).ToList();
            }
        }
        bool IsPassiveUnlocked(Models.MercPassiveDataModel passive) => User.Level >= passive.UnlockLevel;
    }
}