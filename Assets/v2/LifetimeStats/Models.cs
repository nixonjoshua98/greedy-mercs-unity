using Newtonsoft.Json;

namespace GM.PlayerStats
{
    public class LifetimeStatsModel
    {
        public int NumPrestiges;

        [JsonProperty(PropertyName = "highestPrestigeStageReached")]
        public int HighestStage;
    }
}
