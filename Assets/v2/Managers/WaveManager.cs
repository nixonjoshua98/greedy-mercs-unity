using GM.Common;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GM
{
    public class WaveManager : GM.Core.GMMonoBehaviour
    {
        IEnemyUnitFactory UnitManager;

        [HideInInspector] public UnityEvent<UnitFactoryInstantiatedBossUnit> E_BossSpawn { get; private set; } = new UnityEvent<UnitFactoryInstantiatedBossUnit>();
        [HideInInspector] public UnityEvent E_BossDefeated { get; private set; } = new UnityEvent();
        [HideInInspector] public UnityEvent<List<GM.Units.UnitBaseClass>> E_OnWaveStart { get; private set; } = new UnityEvent<List<Units.UnitBaseClass>>();

        CurrentPrestigeState CurrentGameState => App.GMData.GameState;

        void Awake()
        {
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
                enemies.Add(UnitManager.InstantiateEnemyUnit());
            }

            BigDouble combinedHealth = App.GMCache.EnemyHealthAtStage(CurrentGameState.Stage);

            foreach (GM.Units.UnitBaseClass unit in enemies)
            {
                GM.Units.UnitBaseClass unitClass = unit.GetComponent<GM.Units.UnitBaseClass>();

                GM.Controllers.HealthController health = unit.GetComponent<GM.Controllers.HealthController>();

                health.Init(combinedHealth);

                health.E_OnZeroHealth.AddListener(OnEnemyZeroHealth);
            }

            E_OnWaveStart.Invoke(enemies);
        }

        void StartStageBoss()
        {
            UnitFactoryInstantiatedBossUnit enemy = UnitManager.InstantiateEnemyBossUnit();

            // Components
            GM.Controllers.HealthController health = enemy.GameObject.GetComponent<GM.Controllers.HealthController>();
            GM.Units.UnitBaseClass unitClass = enemy.GameObject.GetComponent<GM.Units.UnitBaseClass>();

            // Setup
            health.Init(val: App.GMCache.StageBossHealthAtStage(CurrentGameState.Stage));

            // Add event callbacks
            health.E_OnZeroHealth.AddListener(OnBossZeroHealth);

            // Update the state
            CurrentGameState.HasBossSpawned = true;

            // Set the boss position off-screen
            enemy.GameObject.transform.position = new Vector3(Camera.main.MaxBounds().x + 2.5f, Constants.CENTER_BATTLE_Y);

            E_BossSpawn.Invoke(enemy);
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

            E_BossDefeated.Invoke();

            // Setup the next wave
            SetupWave();
        }
    }
}