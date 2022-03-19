using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace GM.Quests
{
    public class UserQuestsModel
    {
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime LastQuestsRefresh { get; set; }

        public List<int> CompletedMercQuests;
        public List<int> CompletedDailyQuests;
    }
}
