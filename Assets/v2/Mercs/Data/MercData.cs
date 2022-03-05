using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AttackType = GM.Common.Enums.AttackType;
using UnitID = GM.Common.Enums.UnitID;

namespace GM.Mercs.Data
{
    public class MercData : Core.GMClass
    {
        StaticMercData Game;
        UserMercState User;

        public MercData(StaticMercData gameData, UserMercState userData)
        {
            Game = gameData;
            User = userData;
        }

        public UnitID ID => Game.ID;
        public string Name => Game.Name;
        public int CurrentLevel
        {
            get => User.Level; 
            set { User.Level = Mathf.Min(MaxLevel, value); }
        }

        public bool IsMaxLevel => User.Level >= MaxLevel;
        public Sprite Icon => Game.Icon;

        // Energy
        public int EnergyGainedPerSecond => Game.EnergyGainedPerSecond;
        public int SpawnEnergyRequired => GM.Common.Constants.MercSpawnEnergyRequired;
        public int BattleEnergyCapacity => Game.BattleEnergyCapacity;
        public int EnergyConsumedPerAttack => GM.Common.Constants.EnemyConsumedPerAttack;
        public float CurrentSpawnEnergy
        {
            get => User.CurrentSpawnEnergy;
            set => User.CurrentSpawnEnergy = Mathf.Min(SpawnEnergyRequired, value);
        }
        public float CurrentSpawnEnergyPercentage => CurrentSpawnEnergy / SpawnEnergyRequired;


        public int MaxLevel => Common.Constants.MAX_MERC_LEVEL;
        public bool InSquad => User.InSquad;
        public double BaseUpgradeCost => Game.BaseUpgradeCost;
        public AttackType AttackType => Game.AttackType;
        public List<MercPassiveReference> Passives => Game.Passives;
        public BigDouble BaseDamage => Game.BaseDamage;
        public BigDouble DamagePerAttack => App.GMCache.MercDamagePerAttack(this);
        public BigDouble UpgradeCost(int numLevels) => App.GMCache.MercUpgradeCost(this, numLevels);
        public List<MercPassiveReference> UnlockedPassives
        {
            get
            {
                var temp = this; // Linq weirdness

                return Game.Passives.Where(p => temp.IsPassiveUnlocked(p)).ToList();
            }
        }
        public bool IsPassiveUnlocked(MercPassiveReference passive) => User.Level >= passive.UnlockLevel;
    }
}