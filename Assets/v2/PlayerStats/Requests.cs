namespace GM.PlayerStats
{
    public class UpdateLifetimeStatsRequest: GM.HTTP.IServerRequest
    {
        public LifetimeStatsModel Changes;
    }

    public class UpdateLifetimeStatsResponse : GM.HTTP.ServerResponse
    {
        public LifetimeStatsModel LifetimeStats;
    }
}