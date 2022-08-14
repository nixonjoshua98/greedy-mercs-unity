using GM.Controllers;

namespace GM.UserStats
{
    public class PlayerStatsManager : GM.Core.GMMonoBehaviour
    {
        private void Awake()
        {
            GameManager wave = this.GetComponentInScene<GameManager>();
            TapController click = this.GetComponentInScene<TapController>();

            wave.E_OnEnemyDefeated.AddListener(OnEnemyDefeated);
            wave.E_OnBossDefeated.AddListener(OnBossDefeated);

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

        private void OnEnemyDefeated()
        {
            App.Stats.LocalLifetimeStats.TotalEnemiesDefeated++;
            App.Stats.LocalDailyStats.TotalEnemiesDefeated++;
        }

        private void OnBossDefeated()
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
