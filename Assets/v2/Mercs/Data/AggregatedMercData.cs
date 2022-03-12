using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AttackType = GM.Common.Enums.AttackType;
using MercID = GM.Common.Enums.MercID;

namespace GM.Mercs.Data
{
    public class AggregatedMercData : Core.GMClass
    {
        StaticMercData Game;
        UserMercState User;

        public AggregatedMercData(StaticMercData gameData, UserMercState userData)
        {
            Game = gameData;
            User = userData;
        }

        public MercID ID => Game.ID;
        public string Name => Game.Name;
        public int CurrentLevel
        {
            get => User.Level; 
            set { User.Level = Mathf.Min(MaxLevel, value); }
        }

        public bool IsMaxLevel => User.Level >= MaxLevel;
        public Sprite Icon => Game.Icon;

        // Energy
        public int SpawnEnergyRequired => Game.SpawnEnergyRequired;
        public float CurrentSpawnEnergy
        {
            get => User.CurrentSpawnEnergy;
            set => User.CurrentSpawnEnergy = value;
        }

        // Energy (Computed)
        public float CurrentSpawnEnergyPercentage => CurrentSpawnEnergy / SpawnEnergyRequired;

        // Energy (Constants)
        public float EnergyGainedPerSecond => 10;
        public float EnergyConsumedPerAttack => 10;
        public int BattleEnergyCapacity => 50;

        public int MaxLevel => Common.Constants.MAX_MERC_LEVEL;
        public bool InSquad => User.InSquad;
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