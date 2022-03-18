using GM.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace GM.HTTP.Requests
{
    public class FetchGameDataResponse : ServerResponse, IStaticGameData
    {
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime NextDailyReset { get; set; }
        public GM.Quests.StaticQuestsModel Quests { get; set; }
        public List<GM.Artefacts.Models.ArtefactGameDataModel> Artefacts { get; set; }
        public GM.Bounties.Models.CompleteBountyGameDataModel Bounties { get; set; }
        public List<Armoury.Models.ArmouryItemGameDataModel> Armoury { get; set; }
        public GM.Mercs.StaticMercsModel Mercs { get; set; }
    }
}
