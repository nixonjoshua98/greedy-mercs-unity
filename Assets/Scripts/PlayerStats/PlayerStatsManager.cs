using GM.Controllers;

namespace GM.UserStats
{
    public class PlayerStatsManager : GM.Core.GMMonoBehaviour
    {
        private void Awake()
        {
            GameManager wave = this.GetComponentInScene<GameManager>();
            TapController click = this.GetComponentInScene<TapController>();

            wave.E_EnemyDefeated.AddListener(WaveManager_OnEnemyDefeated);
            wave.E_BossDefeated.AddListener(WaveManager_OnBossDefeated);

            click.E_OnTap.AddListener(TapController_OnTap);
        }

        void Start()
        {
            InvokeRepeating(nameof(SyncLifetimeStatsWithServer), 30.0f, 15.0f);
        }

        private void SyncLifetimeStatsWithServer()
        {
            App.Stats.UpdateLifetimeStats(success =>
            {

            });
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

        private void TapController_OnTap()
        {
            App.Stats.LocalLifetimeStats.TotalTaps++;
            App.Stats.LocalDailyStats.TotalTaps++;
        }
    }
}
