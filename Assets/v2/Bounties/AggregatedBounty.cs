using GM.Common.Enums;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GM.Bounties.Models
{
    public class AggregatedBounty : GM.Core.GMClass
    {
        Bounty _bounty;
        UserBounty _userBounty;

        public AggregatedBounty(Bounty gameBounty, UserBounty userBounty)
        {
            _bounty = gameBounty;
            _userBounty = userBounty;
        }

        public int ID => _bounty.ID;

        // User Values //
        public int Level => _userBounty.Level;
        public float BonusValue => CurrentLevelValues?.BonusValue ?? 0.0f;
        public bool CanLevelUp
        {
            get
            {
                var highestLevel = _bounty.Levels.LastOrDefault(x => _userBounty.NumDefeats >= x.NumDefeatsRequired);

                return (_bounty.Levels.IndexOf(highestLevel) + 1) > _userBounty.Level;
            }
        }
        public bool IsMaxLevel => _userBounty.Level >= _bounty.Levels.Count;

        // Bounty Values //
        public BonusType BonusType => _bounty.BonusType;
        public int Income => _bounty.HourlyIncome;
        public string Name => _bounty.Name;
        public Sprite Icon => _bounty.Icon;

        // Private //
        private BountyLevel CurrentLevelValues =>  (Level > 0 && Level <= _bounty.Levels.Count) ? _bounty.Levels[Level - 1] : null;
    }
}
