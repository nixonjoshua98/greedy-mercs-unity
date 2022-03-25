using System.Collections.Generic;

namespace GM.Models
{
    public interface IStaticGameData
    {
        List<Artefacts.Models.ArtefactGameDataModel> Artefacts { get; set; }
        Bounties.Models.CompleteBountyGameDataModel Bounties { get; set; }
        List<Armoury.Models.ArmouryItem> ArmouryItems { get; set; }
        GM.Mercs.StaticMercsModel Mercs { get; set; }
    }

    public class StaticGameDataModel : IStaticGameData
    {
        public List<GM.Artefacts.Models.ArtefactGameDataModel> Artefacts { get; set; }
        public GM.Bounties.Models.CompleteBountyGameDataModel Bounties { get; set; }
        public List<Armoury.Models.ArmouryItem> ArmouryItems { get; set; }
        public GM.Mercs.StaticMercsModel Mercs { get; set; }
    }
}
