using System.Collections.Generic;
using System;

namespace GM.Common.Interfaces
{
    public interface IStaticGameData
    {
        DateTime NextDailyReset { get; set; }

        List<Artefacts.Models.ArtefactGameDataModel> Artefacts { get; set; }
        Bounties.Models.CompleteBountyGameDataModel Bounties { get; set; }
        List<Armoury.Models.ArmouryItemGameDataModel> Armoury { get; set; }
        List<Mercs.Models.MercGameDataModel> Mercs { get; set; }
    }
}