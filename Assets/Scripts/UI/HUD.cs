using TMPro;
using UnityEngine;

namespace GM
{
    public class HUD : Core.GMMonoBehaviour
    {
        [SerializeField]
        private TMP_Text CurrentStageText;
        [SerializeField] private TMP_Text BossNameText;

        private void Awake()
        {
            GameManager wave = this.GetComponentInScene<GameManager>();

            wave.E_BossSpawn.AddListener(WaveManager_BossSpawned);
        }

        private void FixedUpdate()
        {
            CurrentStageText.text = $"Stage {App.GameState.Stage}\n{App.GameState.EnemiesDefeated}/{App.GameState.EnemiesPerStage}";
        }

        private void WaveManager_BossSpawned(SpawnedBoss boss)
        {
            string name = "Boss battle";

            if (boss.BountyID.HasValue)
            {
                name = App.Bounties.GetGameBounty(boss.BountyID.Value).Name;
            }

            BossNameText.text = name;
        }
    }
}