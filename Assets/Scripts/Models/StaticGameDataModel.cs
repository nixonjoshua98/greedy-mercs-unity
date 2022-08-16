using SRC.Armoury.Data;
using System.Collections.Generic;

namespace SRC.Models
{
    public interface IStaticGameData
    {
        List<Artefacts.Data.Artefact> Artefacts { get; set; }
        Bounties.Models.BountiesDataFile Bounties { get; set; }
        List<ArmouryItem> ArmouryItems { get; set; }
        SRC.Mercs.Data.MercDataFile Mercs { get; set; }
    }

    public class StaticGameDataModel : IStaticGameData
    {
        public List<SRC.Artefacts.Data.Artefact> Artefacts { get; set; }
        public SRC.Bounties.Models.BountiesDataFile Bounties { get; set; }
        public List<ArmouryItem> ArmouryItems { get; set; }
        public SRC.Mercs.Data.MercDataFile Mercs { get; set; }
    }
}
