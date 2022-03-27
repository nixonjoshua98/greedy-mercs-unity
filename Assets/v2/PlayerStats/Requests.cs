using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GM.PlayerStats
{
    public class PlayerStatsResponse : GM.HTTP.ServerResponse
    {
        public LifetimeStatsModel LifetimeStats;
        public TimedPlayerStatsModel DailyStats;
    }

    public class UpdateLifetimeStatsRequest: GM.HTTP.IServerRequest
    {
        public LifetimeStatsModel Changes;
    }

    public class UpdateLifetimeStatsResponse : GM.HTTP.ServerResponse
    {
        public LifetimeStatsModel LifetimeStats;
    }
}