using GM.Common;
using GM.Units;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GM
{
    public class WaveManager : GM.Core.GMMonoBehaviour
    {
        /* Scene Components */
        private IEnemyUnitQueue Enemies;
        private IEnemyUnitFactory UnitManager;

        [HideInInspector] public UnityEvent<UnitFactoryInstantiatedBossUnit> E_BossSpawn { get; private set; } = new();
        [HideInInspector] public UnityEvent E_BossDefeated { get; private set; } = new();
        [HideInInspector] public UnityEvent E_EnemyDefeated { get; private set; } = new();
        [HideInInspector] public UnityEvent<List<GM.Units.UnitBaseClass>> E_OnWaveStart { get; private set; } = new();

        private CurrentPrestigeState CurrentGameState => App.GameState;

        private void Awake()
        {
            Enemies = this.GetComponentInScene<IEnemyUnitQueue>();
            UnitManager = this.GetComponentInScene<IEnemyUnitFactory>();
        }

        public void Run()
        {
            if (CurrentGameState.EnemiesRemaining == 0)
            {
                StartStageBoss();
            }
            else
            {
                SetupWave();
            }
        }

        private void SetupWave()
        {
            List<GM.Units.UnitBaseClass> enemies = new List<Units.UnitBaseClass>();

            for (int i = 0; i < CurrentGameState.EnemiesRemaining; i++)
            {
                enemies.Add(UnitManager.InstantiateEnemyUnit());
            }

            BigDouble combinedHealth = App.GMCache.EnemyHealthAtStage(CurrentGameState.Stage);

            foreach (GM.Units.UnitBaseClass unit in enemies)
            {
                GM.Controllers.HealthController health = unit.GetCachedComponent<GM.Controllers.HealthController>();

                health.Init(combinedHealth);

                health.E_OnZeroHealth.AddListener(OnEnemyZeroHealth);
            }

            E_OnWaveStart.Invoke(enemies);
        }

        private void StartStageBoss()
        {
            UnitFactoryInstantiatedBossUnit enemy = UnitManager.InstantiateEnemyBossUnit();

            // Components
            GM.Controllers.HealthController health = enemy.GameObject.GetComponent<GM.Controllers.HealthController>();
            GM.Units.UnitBaseClass unitClass = enemy.GameObject.GetComponent<GM.Units.UnitBaseClass>();

            // Setup
            health.Init(val: App.GMCache.StageBossHealthAtStage(CurrentGameState.Stage));

            // Add event callbacks
            health.E_OnZeroHealth.AddListener(OnBossZeroHealth);

            // Set the boss position off-screen
            enemy.GameObject.transform.position = new Vector3(Camera.main.MaxBounds().x + 2.5f, Constants.CENTER_BATTLE_Y);

            E_BossSpawn.Invoke(enemy);
        }


        // = Event Callbacks = //

        private void OnEnemyZeroHealth()
        {
            CurrentGameState.EnemiesDefeated++;

            E_EnemyDefeated.Invoke();

            // All wave enemies have been defeated
            if (Enemies.NumUnits == 0)
            {
                StartStageBoss();
            }
        }

        private void OnBossZeroHealth()
        {
            CurrentGameState.Stage++;
            CurrentGameState.EnemiesDefeated = 0;

            E_BossDefeated.Invoke();

            // Setup the next wave
            SetupWave();
        }
    }
}