using GM.Common.Enums;
using System.Collections.Generic;
using System.Linq;
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

        UserMercLocalState LocalState => App.Mercs.GetLocalStateOrNull(ID);
        UserMercState State => App.Mercs.GetUserStateOrNull(ID);

        public MercID ID => StaticValues.ID;
        public string Name => StaticValues.Name;
        public GameObject Prefab => StaticValues.Prefab;
        public Sprite Icon => StaticValues.Icon;
        public float RechargeRate => StaticValues.RechargeRate;
        public float RechargeProgress { get => LocalState.RechargeProgress; set => LocalState.RechargeProgress = value; }
        public float RechargePercentage => LocalState.RechargeProgress / StaticValues.RechargeRate;
        public float EnergyConsumedPerAttack => 10;
        public int BattleEnergyCapacity => 50;
        public bool IsUnlocked => State is not null;
        public int CurrentLevel { get => LocalState.Level; set => LocalState.Level = Mathf.Min(MaxLevel, value); }
        public int MaxLevel => 1_000;
        public bool IsMaxLevel => LocalState.Level >= MaxLevel;
        public bool InSquad => App.Mercs.InSquad(ID);
        public UnitAttackType AttackType => StaticValues.AttackType;
        public List<MercPassive> Passives => StaticValues.Passives;
        public List<MercPassive> UnlockedPassives => StaticValues.Passives.Where(x => CurrentLevel >= x.UnlockLevel).ToList();
        public float BaseDamage => StaticValues.BaseDamage;
        public BigDouble DamagePerAttack => App.Values.MercDamagePerAttack(this);
        public BigDouble AttackDamage(int level) => App.Values.MercDamagePerAttack(this, level);
        public BigDouble UpgradeCost(int numLevels) => App.Values.MercUpgradeCost(this, numLevels);
    }
}