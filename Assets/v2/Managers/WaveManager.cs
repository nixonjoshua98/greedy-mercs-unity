using GM.Common;
using GM.States;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GM
{
    public class WaveManager : GM.Core.GMMonoBehaviour
    {
        IEnemyUnitFactory UnitManager;

        [HideInInspector] public UnityEvent<GM.Units.UnitBaseClass> E_BossSpawn { get; private set; } = new UnityEvent<GM.Units.UnitBaseClass>();
        [HideInInspector] public UnityEvent<List<GM.Units.UnitBaseClass>> E_OnWaveSpawn { get; private set; } = new UnityEvent<List<Units.UnitBaseClass>>();

        GameState CurrentGameState => App.Data.GameState;

        void Awake()
        {
            // Fetch required components
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

        void SetupWave()
        {
            List<GM.Units.UnitBaseClass> enemies = new List<Units.UnitBaseClass>();

            for (int i = 0; i < CurrentGameState.EnemiesRemaining; i++)
            {
                enemies.Add(UnitManager.InstantiateEnemyUnit().GetComponent<Units.UnitBaseClass>());
            }

            BigDouble combinedHealth = App.Cache.EnemyHealthAtStage(CurrentGameState.Stage);

            foreach (GM.Units.UnitBaseClass unit in enemies)
            {
                GM.Units.UnitBaseClass unitClass = unit.GetComponent<GM.Units.UnitBaseClass>();

                GM.Controllers.HealthController health = unit.GetComponent<GM.Controllers.HealthController>();

                health.Init(combinedHealth);

                health.E_OnZeroHealth.AddListener(OnEnemyZeroHealth);
            }

            E_OnWaveSpawn.Invoke(enemies);
        }

        void StartStageBoss()
        {
            GameObject enemy = UnitManager.InstantiateEnemyBossUnit();

            // Components
            GM.Controllers.HealthController health = enemy.GetComponent<GM.Controllers.HealthController>();
            GM.Units.UnitBaseClass unitClass = enemy.GetComponent<GM.Units.UnitBaseClass>();

            // Setup
            health.Init(val: App.Cache.StageBossHealthAtStage(CurrentGameState.Stage));

            // Add event callbacks
            health.E_OnZeroHealth.AddListener(OnBossZeroHealth);

            // Update the state
            CurrentGameState.HasBossSpawned = true;

            // Set the boss position off-screen
            enemy.transform.position = new Vector3(Camera.main.MaxBounds().x + 2.5f, Constants.CENTER_BATTLE_Y);

            // Invoke an event
            E_BossSpawn.Invoke(unitClass);
        }


        // = Event Callbacks = //

        void OnEnemyZeroHealth()
        {
            CurrentGameState.EnemiesDefeated++;

            // All wave enemies have been defeated
            if (UnitManager.NumEnemyUnits == 0)
            {
                StartStageBoss();
            }
        }

        void OnBossZeroHealth()
        {
            // Update the state, mainly used for saving and loading state
            CurrentGameState.HasBossSpawned = false;

            CurrentGameState.Stage++;
            CurrentGameState.EnemiesDefeated = 0;

            // Setup the next wave
            SetupWave();
        }
    }
}
