using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using GM.Common.Json;

namespace GM.HTTP.Requests
{
    public class FetchGameDataResponse : ServerResponse, Common.ICompleteGameData
    {
        [JsonConverter(typeof(UnixMillisecondUTCDateTimeConverter))]
        public DateTime NextDailyReset { get; set; }
        public List<GM.Artefacts.Models.ArtefactGameDataModel> Artefacts { get; set; }
        public GM.Bounties.Models.AllBountyGameDataModel Bounties { get; set; }
        public GM.Armoury.Models.AllArmouryGameDataModel Armoury { get; set; }
        public List<GM.Mercs.Models.MercGameDataModel> Mercs { get; set; }
    }
}
