using System.Collections.Generic;
using System;

namespace GM.Common
{
    public interface IServerGameData
    {
        DateTime NextDailyReset { get; set; }

        List<GM.Artefacts.Models.ArtefactGameDataModel> Artefacts { get; set; }
        GM.Bounties.Models.CompleteBountyGameDataModel Bounties { get; set; }
        GM.Armoury.Models.ArmouryGameDataModel Armoury { get; set; }
        List<GM.Mercs.Models.MercGameDataModel> Mercs { get; set; }
    }
}