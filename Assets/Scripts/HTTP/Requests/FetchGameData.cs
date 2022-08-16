using SRC.Armoury.Data;
using SRC.Models;
using System.Collections.Generic;

namespace SRC.HTTP.Requests
{
    public class FetchGameDataResponse : ServerResponse, IStaticGameData
    {
        public List<SRC.Artefacts.Data.Artefact> Artefacts { get; set; }
        public SRC.Bounties.Models.BountiesDataFile Bounties { get; set; }
        public List<ArmouryItem> ArmouryItems { get; set; }
        public SRC.Mercs.Data.MercDataFile Mercs { get; set; }
    }
}
