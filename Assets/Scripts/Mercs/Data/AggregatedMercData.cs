using SRC.Common.Enums;
using SRC.Mercs.ScriptableObjects;
using SRC.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SRC.Mercs.Data
{
    public partial class AggregatedMercData : Core.GMClass
    {
        public MercID MercID { get; private set; }

        private readonly Func<MercGameData> GetMercGameData;
        private readonly Func<MercLocalUserData> GetLocalUserData;
        private readonly Func<MercLocalDataFileMerc> GetLocalGameData;

        private MercGameData GameData => GetMercGameData();
        private MercLocalUserData LocalState => GetLocalUserData();
        private MercLocalDataFileMerc LocalGameData => GetLocalGameData();

        public AggregatedMercData(MercID mercId, Func<MercGameData> getMercGameData, Func<MercLocalUserData> getLocalUserData, Func<MercLocalDataFileMerc> getLocalGameData)
        {
            MercID = mercId;

            GetMercGameData = getMercGameData;
            GetLocalUserData = getLocalUserData;
            GetLocalGameData = getLocalGameData;
        }
    }

    public partial class AggregatedMercData : Core.GMClass
    {
        public string Name => GameData.Name;
        public GameObject Prefab => LocalGameData.Prefab;
        public Sprite Icon => LocalGameData.Icon;
        public int CurrentLevel { get => LocalState.Level; set => LocalState.Level = Mathf.Min(MaxLevel, value); }
        public int MaxLevel => 1_000;
        public ItemGradeData ItemGrade => App.Local.GetItemGrade(GameData.Grade);
        public bool IsMaxLevel => LocalState.Level >= MaxLevel;
        public UnitAttackType AttackType => GameData.AttackType;
        public List<MercPassive> Passives => GameData.Passives;
        public List<MercPassive> UnlockedPassives => GameData.Passives.Where(x => CurrentLevel >= x.UnlockLevel).ToList();
        public float BaseDamage => GameData.BaseDamage;
        public BigDouble DamagePerAttack => App.Values.MercDamagePerAttack(this);
        public BigDouble AttackDamage(int level) => App.Values.MercDamagePerAttack(this, level);
        public bool IsPassiveUnlocked(int passiveId) => UnlockedPassives.FirstOrDefault(x => x.PassiveID == passiveId) is not null;
        public BigDouble UpgradeCost(int numLevels) => App.Values.MercUpgradeCost(this, numLevels);
    }
}