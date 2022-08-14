using GM.Armoury.Data;
using GM.Models;
using System.Collections.Generic;

namespace GM.HTTP.Requests
{
    public class FetchGameDataResponse : ServerResponse, IStaticGameData
    {
        public List<GM.Artefacts.Data.Artefact> Artefacts { get; set; }
        public GM.Bounties.Models.BountiesDataModel Bounties { get; set; }
        public List<ArmouryItem> ArmouryItems { get; set; }
        public GM.Mercs.Data.MercDataFile Mercs { get; set; }
    }
}
