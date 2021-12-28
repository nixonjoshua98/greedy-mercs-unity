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

        public MercID Id => Game.Id;
        public string Name => Game.Name;
        public int CurrentLevel
        {
            get => User.Level; 
            set
            {
                if (value > MaxLevel)
                    GMLogger.Log($"Fatal: Merc '{Id}' level exceed max level");

                User.Level = Mathf.Min(MaxLevel, value);
            }
        }
        public bool IsMaxLevel => User.Level >= MaxLevel;
        public Sprite Icon => Game.Icon;
        public int MaxLevel => Common.Constants.MAX_MERC_LEVEL;
        public bool InSquad => User.InSquad;
        public double UnlockCost => Game.UnlockCost;
        public bool IsDefault => Game.IsDefault;
        public AttackType AttackType => Game.AttackType;
        public Models.MercPassiveDataModel[] Passives => Game.Passives;
        public BigDouble BaseDamage => Game.BaseDamage;
        public BigDouble DamagePerAttack => App.Cache.MercDamagePerAttack(this);
        public BigDouble UpgradeCost(int numLevels) => App.Cache.MercUpgradeCost(this, numLevels);
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