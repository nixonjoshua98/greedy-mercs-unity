using SRC.Common.Enums;
using System.Collections.Generic;

namespace SRC.Bounties.Models
{
    public class BountyLevel
    {
        public int Level;
        public int KillsRequired;
        public float BonusValue;
    }

    public class Bounty
    {
        public int BountyID;
        public string Name;
        public int PointsPerHour;
        public BonusType BonusType;
        public Rarity Tier;
        public string Description;
        public int UnlockStage;
        public List<BountyLevel> Levels = new();
    }

    public class BountiesDataFile
    {
        public float MaxUnclaimedHours;
        public int MaxActiveBounties;

        public List<Bounty> Bounties;
    }
}
