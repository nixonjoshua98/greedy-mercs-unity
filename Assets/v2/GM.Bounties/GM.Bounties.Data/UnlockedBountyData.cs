using GM.Bounties.Models;
using UnityEngine;

namespace GM.Bounties.Data
{
    public struct UnlockedBountyData
    {
        BountyGameData _GameData;
        BountyUserDataModel _UserData;

        public UnlockedBountyData(BountyGameData gameBounty, BountyUserDataModel userBounty)
        {
            _GameData = gameBounty;
            _UserData = userBounty;
        }

        public int Id => _UserData.BountyId;
        public bool IsActive => _UserData.IsActive;
        public string Name => _GameData.Name;
        public Sprite Icon => _GameData.Icon;
        public long Income => _GameData.HourlyIncome;
    }
}
