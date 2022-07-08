using GM.Armoury.Data;
using System.Collections.Generic;

namespace GM.Models
{
    public interface IStaticGameData
    {
        List<Artefacts.Models.Artefact> Artefacts { get; set; }
        Bounties.Models.BountiesDataModel Bounties { get; set; }
        List<ArmouryItem> ArmouryItems { get; set; }
        GM.Mercs.StaticMercsModel Mercs { get; set; }
    }

    public class StaticGameDataModel : IStaticGameData
    {
        public List<GM.Artefacts.Models.Artefact> Artefacts { get; set; }
        public GM.Bounties.Models.BountiesDataModel Bounties { get; set; }
        public List<ArmouryItem> ArmouryItems { get; set; }
        public GM.Mercs.StaticMercsModel Mercs { get; set; }
    }
}
