using GM.Bounties.Models;
using UnityEngine;

namespace GM.Bounties.Data
{
    public class AggregatedBounty : GM.Core.GMClass
    {
        BountyGameData _GameData;
        UserBounty _UserData;

        public AggregatedBounty(BountyGameData gameBounty, UserBounty userBounty)
        {
            _GameData = gameBounty;
            _UserData = userBounty;
        }

        public int Id => _UserData.BountyID;
        public bool IsActive => App.Bounties.IsBountyActive(_UserData.BountyID);
        public string Name => _GameData.Name;
        public Sprite Icon => _GameData.Icon;
        public long Income => _GameData.HourlyIncome;
    }
}
