using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using BonusType = GM.Common.Enums.BonusType;
using MercID = GM.Common.Enums.MercID;
using AttackType = GM.Common.Enums.AttackType;

namespace GM.Mercs.Data
{
    public class FullMercData : Core.GMClass
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
        public AttackType AttackType => Game.AttackType;
        public Models.MercPassiveDataModel[] Passives => Game.Passives;
        public BigDouble BaseDamage => Game.BaseDamage;
        public Dictionary<BonusType, double> BonusesFromPassives => App.Cache.BonusesFromMercPassives(this);
        public List<Models.MercPassiveDataModel> UnlockedPassives
        {
            get
            {
                var temp = this; // Linq weirdness

                return Game.Passives.Where(p => temp.IsPassiveUnlocked(p)).ToList();
            }
        }
        public bool IsPassiveUnlocked(Models.MercPassiveDataModel passive) => User.Level >= passive.UnlockLevel;
    }
}