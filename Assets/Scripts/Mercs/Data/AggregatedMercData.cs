using System.Collections.Generic;
using UnityEngine;
using GM.Common.Enums;

namespace GM.Mercs.Data
{
    public class AggregatedMercData : Core.GMClass
    {
        StaticMercData StaticValues;

        public AggregatedMercData(StaticMercData gameData)
        {
            StaticValues = gameData;
        }

        UserMercState State
        {
            get
            {
                App.Mercs.TryGetMercState(StaticValues.ID, out UserMercState state);
                return state;
            }
        }

        public MercID ID => StaticValues.ID;
        public string Name => StaticValues.Name;
        public GameObject Prefab => StaticValues.Prefab;
        public Sprite Icon => StaticValues.Icon;
        public int SpawnEnergyRequired => StaticValues.SpawnEnergyRequired;
        public float CurrentSpawnEnergy { get => State.CurrentSpawnEnergy; set => State.CurrentSpawnEnergy = value; }
        public float CurrentSpawnEnergyPercentage => CurrentSpawnEnergy / SpawnEnergyRequired;
        public float EnergyGainedPerSecond => 10;
        public float EnergyConsumedPerAttack => 10;
        public int BattleEnergyCapacity => 50;
        public bool IsUnlocked => State is not null;
        public int CurrentLevel { get => State.Level; set => State.Level = Mathf.Min(MaxLevel, value); }
        public int MaxLevel => 1_000;
        public bool IsMaxLevel => State.Level >= MaxLevel;
        public bool InSquad => App.PersistantLocalFile.SquadMercIDs.Contains(StaticValues.ID);
        public AttackType AttackType => StaticValues.AttackType;
        public List<MercPassiveReference> Passives => StaticValues.Passives;
        public BigDouble BaseDamage => StaticValues.BaseDamage;
        public BigDouble DamagePerAttack => App.GMCache.MercDamagePerAttack(this);
        public BigDouble UpgradeCost(int numLevels) => App.GMCache.MercUpgradeCost(this, numLevels);
        public bool IsPassiveUnlocked(MercPassiveReference passive) => State.Level >= passive.UnlockLevel;
    }
}