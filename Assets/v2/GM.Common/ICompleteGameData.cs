using System.Collections.Generic;
using System;

namespace GM.Common
{
    public interface ICompleteGameData
    {
        DateTime NextDailyReset { get; set; }

        List<GM.Artefacts.Models.ArtefactGameDataModel> Artefacts { get; set; }
        GM.Bounties.Models.AllBountyGameDataModel Bounties { get; set; }
        GM.Armoury.Models.AllArmouryGameDataModel Armoury { get; set; }
        List<GM.Mercs.Models.MercGameDataModel> Mercs { get; set; }
    }
}