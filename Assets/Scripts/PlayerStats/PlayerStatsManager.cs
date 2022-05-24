using GM.Controllers;

namespace GM.PlayerStats
{
    public class PlayerStatsManager : GM.Core.GMMonoBehaviour
    {
        private void Awake()
        {
            GameManager wave = this.GetComponentInScene<GameManager>();
            TapController click = this.GetComponentInScene<TapController>();

            wave.E_EnemyDefeated.AddListener(WaveManager_OnEnemyDefeated);
            wave.E_BossDefeated.AddListener(WaveManager_OnBossDefeated);

            click.E_OnTap.AddListener(TapController_OnTop);
        }

        private void WaveManager_OnEnemyDefeated()
        {
            App.Stats.LocalLifetimeStats.TotalEnemiesDefeated++;
            App.Stats.LocalDailyStats.TotalEnemiesDefeated++;
        }

        private void WaveManager_OnBossDefeated()
        {
            App.Stats.LocalLifetimeStats.TotalBossesDefeated++;
            App.Stats.LocalDailyStats.TotalBossesDefeated++;
        }

        private void TapController_OnTop()
        {
            App.Stats.LocalLifetimeStats.TotalTaps++;
            App.Stats.LocalDailyStats.TotalTaps++;
        }
    }
}
