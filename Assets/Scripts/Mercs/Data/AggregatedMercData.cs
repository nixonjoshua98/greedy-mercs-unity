using System.Collections.Generic;
using UnityEngine;
using AttackType = GM.Common.Enums.AttackType;
using MercID = GM.Common.Enums.MercID;

namespace GM.Mercs.Data
{
    public class AggregatedMercData : Core.GMClass
    {
        private readonly StaticMercData Game;
        private readonly UserMercState User;

        public AggregatedMercData(StaticMercData gameData, UserMercState userData)
        {
            Game = gameData;
            User = userData;
        }

        public MercID ID => Game.ID;
        public string Name => Game.Name;
        public Sprite Icon => Game.Icon;

        // Energy
        public int SpawnEnergyRequired => Game.SpawnEnergyRequired;
        public float CurrentSpawnEnergy { get => User.CurrentSpawnEnergy; set => User.CurrentSpawnEnergy = value; }

        // Energy (Computed)
        public float CurrentSpawnEnergyPercentage => CurrentSpawnEnergy / SpawnEnergyRequired;

        // Energy (Constants)
        public float EnergyGainedPerSecond => 10;
        public float EnergyConsumedPerAttack => 10;
        public int BattleEnergyCapacity => 50;

        // Level
        public int CurrentLevel { get => User.Level; set => User.Level = Mathf.Min(MaxLevel, value); }

        // Level (Constants)
        public int MaxLevel => 1_000;

        // Level (Computed)
        public bool IsMaxLevel => User.Level >= MaxLevel;

        public bool InSquad => App.PersistantLocalFile.SquadMercIDs.Contains(Game.ID);
        public AttackType AttackType => Game.AttackType;
        public List<MercPassiveReference> Passives => Game.Passives;
        public BigDouble BaseDamage => Game.BaseDamage;
        public BigDouble DamagePerAttack => App.GMCache.MercDamagePerAttack(this);
        public BigDouble UpgradeCost(int numLevels)
        {
            return App.GMCache.MercUpgradeCost(this, numLevels);
        }

        public bool IsPassiveUnlocked(MercPassiveReference passive)
        {
            return User.Level >= passive.UnlockLevel;
        }
    }
}