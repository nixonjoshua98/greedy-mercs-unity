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
            WaveManager wave = this.GetComponentInScene<WaveManager>();

            wave.E_BossSpawn.AddListener(WaveManager_BossSpawned);
        }

        private void FixedUpdate()
        {
            CurrentStageText.text = $"Stage {App.GameState.Stage}\n{App.GameState.EnemiesDefeated}/{App.GameState.EnemiesPerStage}";
        }

        private void WaveManager_BossSpawned(UnitFactoryInstantiatedBossUnit boss)
        {
            BossNameText.text = boss.Name;
        }
    }
}