using UnityEngine;

namespace GM.PlayerStats
{
    public class StatsDataContainer : GM.Core.GMClass
    {
        LifetimeStatsModel Lifetime;

        // = Lifetime Properties = //
        public int LifetimePrestiges => Lifetime.NumPrestiges;
        public int LifetimeHighestStage => Mathf.Max(App.GameState.Stage, Lifetime.HighestStage);

        public void Set(LifetimeStatsModel userData)
        {
            Lifetime = userData;
        }
    }
}
