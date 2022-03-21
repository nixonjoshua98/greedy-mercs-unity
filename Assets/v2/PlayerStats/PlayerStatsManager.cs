namespace GM.PlayerStats
{
    public class PlayerStatsManager : GM.Core.GMMonoBehaviour
    {
        void Awake()
        {
            WaveManager wave = this.GetComponentInScene<WaveManager>();

            wave.E_EnemyDefeated.AddListener(WaveManager_OnEnemyDefeated);
            wave.E_BossDefeated.AddListener(WaveManager_OnBossDefeated);
        }

        void WaveManager_OnEnemyDefeated()
        {
            App.Stats.LocalDailyStats.TotalEnemiesDefeated++;
        }

        void WaveManager_OnBossDefeated()
        {
            App.Stats.LocalDailyStats.TotalBossesDefeated++;
        }
    }
}
