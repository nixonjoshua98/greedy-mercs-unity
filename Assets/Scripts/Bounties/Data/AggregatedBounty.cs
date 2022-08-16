using SRC.Common.Enums;
using SRC.Bounties.Scriptables;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SRC.Bounties.Models
{
    public partial class AggregatedBounty
    {
        public int BountyID { get; private set; }

        private readonly Func<UserBounty> GetUserState;
        private readonly Func<Bounty> GetDataFile;
        private readonly Func<BountiesLocalDataFileBounty> GetLocalDataFile;

        private Bounty DataFile => GetDataFile();
        private UserBounty UserState => GetUserState();
        private BountiesLocalDataFileBounty LocalDataFile => GetLocalDataFile();

        public AggregatedBounty(int bountyId, Func<Bounty> datafile, Func<BountiesLocalDataFileBounty> getLocalDataFile, Func<UserBounty> getUserState)
        {
            BountyID = bountyId;
            GetDataFile = datafile;
            GetLocalDataFile = getLocalDataFile;
            GetUserState = getUserState;
        }
    }

    public partial class AggregatedBounty
    {
        public int Level => UserState.Level;
        public int CurrentKillCount => UserState.CurrentKillCount;
        public float BonusValue => CurrentLevel?.BonusValue ?? 0.0f;
        public bool CanUpgrade => DataFile.Levels.LastOrDefault(x => UserState.CurrentKillCount >= x.KillsRequired).Level > UserState.Level;
        public bool IsMaxLevel => UserState.Level >= DataFile.Levels.Count;
        public string Description => DataFile.Description;
        public List<BountyLevel> Levels => DataFile.Levels;
        public BonusType BonusType => DataFile.BonusType;
        public int PointsPerHour => DataFile.PointsPerHour;
        public float NextUpgradeProgressPercentage => !IsMaxLevel ? (CurrentKillCount / (float)Levels[Level].KillsRequired) : 1.0f;
        public Rarity Tier => DataFile.Tier;
        public int Stage => DataFile.UnlockStage;
        public GameObject Prefab => LocalDataFile.Prefab;
        public string Name => DataFile.Name;
        public Sprite Icon => LocalDataFile.Icon;

        public BountyLevel NextLevel => DataFile.Levels.FirstOrDefault(lvl => lvl.Level == Level + 1);
        public BountyLevel CurrentLevel => DataFile.Levels.FirstOrDefault(lvl => lvl.Level == Level);
    }
}