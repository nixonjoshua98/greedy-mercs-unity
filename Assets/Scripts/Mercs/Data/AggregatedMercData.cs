using GM.Common.Enums;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Mercs.Data
{
    public class AggregatedMercData : Core.GMClass
    {
        readonly StaticMercData StaticValues;

        public AggregatedMercData(StaticMercData gameData)
        {
            StaticValues = gameData;
        }

        UserMercState State => App.Mercs.GetStateOrNull(ID);

        public MercID ID => StaticValues.ID;
        public string Name => StaticValues.Name;
        public GameObject Prefab => StaticValues.Prefab;
        public Sprite Icon => StaticValues.Icon;
        public float RechargeRate => StaticValues.RechargeRate;
        public float RechargeProgress { get => State.RechargeProgress; set => State.RechargeProgress = value; }
        public float RechargePercentage => State.RechargeProgress / StaticValues.RechargeRate;
        public float EnergyConsumedPerAttack => 10;
        public int BattleEnergyCapacity => 50;
        public bool IsUnlocked => State is not null;
        public int CurrentLevel { get => State.Level; set => State.Level = Mathf.Min(MaxLevel, value); }
        public int MaxLevel => 1_000;
        public bool IsMaxLevel => State.Level >= MaxLevel;
        public bool InSquad => App.Mercs.InSquad(ID);
        public UnitAttackType AttackType => StaticValues.AttackType;
        public List<MercPassiveReference> Passives => StaticValues.Passives;
        public float BaseDamage => StaticValues.BaseDamage;
        public BigDouble DamagePerAttack => App.Values.MercDamagePerAttack(this);
        public BigDouble AttackDamage(int level) => App.Values.MercDamagePerAttack(this, level);
        public BigDouble UpgradeCost(int numLevels) => App.Values.MercUpgradeCost(this, numLevels);
    }
}