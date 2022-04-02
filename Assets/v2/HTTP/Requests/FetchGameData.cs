using GM.Models;
using System.Collections.Generic;

namespace GM.HTTP.Requests
{
    public class FetchGameDataResponse : ServerResponse, IStaticGameData
    {
        public List<GM.Artefacts.Models.ArtefactGameDataModel> Artefacts { get; set; }
        public GM.Bounties.Models.BountiesDataModel Bounties { get; set; }
        public List<Armoury.Models.ArmouryItem> ArmouryItems { get; set; }
        public GM.Mercs.StaticMercsModel Mercs { get; set; }
    }
}
