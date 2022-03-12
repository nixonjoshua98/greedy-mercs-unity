using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace GM.Models
{
    public interface IStaticGameData
    {
        DateTime NextDailyReset { get; set; }

        GM.Quests.StaticQuestsModel Quests { get; set; }
        List<Artefacts.Models.ArtefactGameDataModel> Artefacts { get; set; }
        Bounties.Models.CompleteBountyGameDataModel Bounties { get; set; }
        List<Armoury.Models.ArmouryItemGameDataModel> Armoury { get; set; }
        GM.Mercs.StaticMercsDataResponse Mercs { get; set; }
    }

    public class StaticGameDataModel : IStaticGameData
    {
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime NextDailyReset { get; set; }

        public GM.Quests.StaticQuestsModel Quests { get; set; }
        public List<GM.Artefacts.Models.ArtefactGameDataModel> Artefacts { get; set; }
        public GM.Bounties.Models.CompleteBountyGameDataModel Bounties { get; set; }
        public List<Armoury.Models.ArmouryItemGameDataModel> Armoury { get; set; }
        public GM.Mercs.StaticMercsDataResponse Mercs { get; set; }
    }
}
