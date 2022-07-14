using GM.Enums;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GM.Bounties.Models
{
    public class AggregatedBounty : GM.Core.GMClass
    {
        private readonly Bounty _bounty;
        private readonly UserBounty _userBounty;

        public AggregatedBounty(Bounty gameBounty, UserBounty userBounty)
        {
            _bounty = gameBounty;
            _userBounty = userBounty;
        }

        public int ID => _bounty.ID;
        public int Level => _userBounty.Level;
        public int NumDefeats => _userBounty.NumDefeats;
        public float BonusValue => CurrentLevelValues?.BonusValue ?? 0.0f;
        public bool CanUpgrade
        {
            get
            {
                var highestLevel = _bounty.Levels.LastOrDefault(x => _userBounty.NumDefeats >= x.NumDefeatsRequired);

                return (_bounty.Levels.IndexOf(highestLevel) + 1) > _userBounty.Level;
            }
        }
        public bool IsMaxLevel => _userBounty.Level >= _bounty.Levels.Count;
        public string Description => _bounty.Description;
        public List<BountyLevel> Levels => _bounty.Levels;
        public BonusType BonusType => _bounty.BonusType;
        public int Income => _bounty.HourlyIncome;
        public string Name => _bounty.Name;
        public Sprite Icon => _bounty.Icon;
        public bool IsActive => App.Bounties.IsBountyActive(_bounty.ID);

        private BountyLevel CurrentLevelValues => (Level > 0 && Level <= _bounty.Levels.Count) ? _bounty.Levels[Level - 1] : null;
    }
}