using TMPro;
using UnityEngine;

namespace GM
{
    public class HUD : Core.GMMonoBehaviour
    {
        [SerializeField]
        TMP_Text CurrentStageText;
        [SerializeField] TMP_Text BossNameText;

        void Awake()
        {
            WaveManager wave = this.GetComponentInScene<WaveManager>();

            wave.E_BossSpawn.AddListener(WaveManager_BossSpawned);
        }

        void FixedUpdate()
        {
            CurrentStageText.text = $"Stage {App.GameState.Stage}\n{App.GameState.EnemiesDefeated}/{App.GameState.EnemiesPerStage}";
        }

        void WaveManager_BossSpawned(UnitFactoryInstantiatedBossUnit boss)
        {
            BossNameText.text = boss.Name;
        }
    }
}