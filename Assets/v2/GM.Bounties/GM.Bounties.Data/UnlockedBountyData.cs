using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;
using GM.Bounties.Models;

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
        public float Income => _GameData.HourlyIncome;
    }
}
