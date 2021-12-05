using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AttackType = GM.Common.Enums.AttackType;
using MercID = GM.Common.Enums.MercID;

namespace GM.Mercs.Data
{
    public class MercData : Core.GMClass
    {
        Models.MercGameDataModel Game;
        MercUserData User;

        public MercData(Models.MercGameDataModel gameData, MercUserData userData)
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
        public bool IsMaxLevel => User.Level >= Common.Constants.MAX_MERC_LEVEL;
        public Sprite Icon => Game.Icon;
        public double UnlockCost => Game.UnlockCost;
        public AttackType AttackType => Game.AttackType;
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
        public bool IsPassiveUnlocked(Models.MercPassiveDataModel passive) => User.Level >= passive.UnlockLevel;
    }
}