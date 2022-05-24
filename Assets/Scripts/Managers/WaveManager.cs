using GM.Bounties.Models;
using GM.Common;
using GM.Controllers;
using GM.Units;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GM
{
    public class WaveManager : GM.Core.GMMonoBehaviour, IEnemyUnitCollection
    {
        [Header("Prefabs")]
        public GameObject EnemyObject;
        public GameObject BossObject;

        [Header("Events")]
        [HideInInspector] public UnityEvent<SpawnedBoss> E_BossSpawn = new();
        [HideInInspector] public UnityEvent E_BossDefeated = new();
        [HideInInspector] public UnityEvent E_EnemyDefeated = new();

        private List<UnitBase> SpawnedEnemies = new List<UnitBase>();

        /// <summary>
        /// Entry point into the system. Once has been called then the wave system will take over
        /// </summary>
        public void Run()
        {
            if (App.GameState.EnemiesRemaining == 0)
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
            BigDouble maxHealth = App.GMCache.EnemyHealthAtStage(App.GameState.Stage);

            for (int i = 0; i < App.GameState.EnemiesRemaining; i++)
            {
                GameObject enemy = InstantiateEnemyUnit();

                UnitBase unit = enemy.GetComponent<UnitBase>(); 

                HealthController health = unit.GetComponent<HealthController>();

                health.Init(maxHealth);
            }
        }

        /// <summary>
        /// Instantiate a regular enemy unit with minimal setup
        /// </summary>
        public GameObject InstantiateEnemyUnit()
        {
            GameObject obj = Instantiate(EnemyObject, EnemyUnitSpawnPosition(), Quaternion.identity);

            // Components
            UnitBase unit = obj.GetComponent<UnitBase>();
            AbstractHealthController health = obj.GetComponent<AbstractHealthController>();

            health.Invincible = true;

            health.E_OnZeroHealth.AddListener(() => OnEnemyZeroHealth(obj));

            SpawnedEnemies.Add(unit);

            // Unit cannot be attacked until they are visible on screen
            Enumerators.InvokeAfter(this, () => Camera.main.IsVisible(unit.Avatar.Bounds.min), () => health.Invincible = false);

            return obj;
        }

        /// <summary>
        /// Instantiate a enemy unit boss variant
        /// </summary>
        public SpawnedBoss InstantiateEnemyBossUnit()
        {
            GameObject unitToSpawn = App.Bounties.TryGetStageBounty(App.GameState.Stage, out Bounty bountyData) ? bountyData.Prefab : BossObject;

            // Instantiate the boss object
            GameObject bossObject = Instantiate(unitToSpawn, EnemyUnitSpawnPosition(), Quaternion.identity);

            // Components
            UnitBase unit = bossObject.GetComponent<UnitBase>();
            AbstractHealthController health = bossObject.GetComponent<AbstractHealthController>();

            // Set the enemy to be invinsible while it is not visible on screen
            health.Invincible = true;

            health.E_OnZeroHealth.AddListener(() => OnBossZeroHealth(bossObject));

            SpawnedEnemies.Add(unit);

            // Unit cannot be attacked until they are visible on screen
            Enumerators.InvokeAfter(this, () => Camera.main.IsVisible(bossObject.transform.position), () => health.Invincible = false);

            return new() { GameObject = bossObject, BountyID = bountyData?.ID};
        }

        /// <summary>
        /// Calculate the next unit spawn position
        /// </summary>
        private Vector3 EnemyUnitSpawnPosition()
        {
            if (SpawnedEnemies.Count > 0)
            {
                UnitBase unit = SpawnedEnemies[SpawnedEnemies.Count - 1];

                return new Vector3(unit.Avatar.Bounds.max.x + (unit.Avatar.Bounds.size.x / 2) + 1.0f, unit.transform.position.y);
            }

            return new Vector3(8, GM.Common.Constants.CENTER_BATTLE_Y) + new Vector3(Camera.main.MaxBounds().x, 0);
        }

        public bool TryGetUnit(ref UnitBase current)
        {
            if (!ContainsUnit(current) && SpawnedEnemies.Count > 0)
            {
                current = SpawnedEnemies[0];
            }

            return ContainsUnit(current);
        }

        public bool ContainsUnit(UnitBase unit)
        {
            return SpawnedEnemies.Contains(unit);
        }

        private void StartStageBoss()
        {
            SpawnedBoss enemy = InstantiateEnemyBossUnit();

            // Components
            HealthController health = enemy.GameObject.GetComponent<GM.Controllers.HealthController>();

            // Setup
            health.Init(val: App.GMCache.StageBossHealthAtStage(App.GameState.Stage));

            // Set the boss position off-screen
            enemy.GameObject.transform.position = new Vector3(Camera.main.MaxBounds().x + 2.5f, Constants.CENTER_BATTLE_Y);

            E_BossSpawn.Invoke(enemy);
        }

        void OnEnemyZeroHealth(GameObject unitObject)
        {
            UnitBase unit = unitObject.GetComponent<UnitBase>();

            SpawnedEnemies.Remove(unit);

            App.GameState.EnemiesDefeated++;

            E_EnemyDefeated.Invoke();

            if (SpawnedEnemies.Count == 0)
            {
                StartStageBoss();
            }
        }

        void OnBossZeroHealth(GameObject unitObject)
        {
            UnitBase unit = unitObject.GetComponent<UnitBase>();

            SpawnedEnemies.Remove(unit);

            App.GameState.Stage++;
            App.GameState.EnemiesDefeated = 0;

            E_BossDefeated.Invoke();

            SetupWave();
        }
    }
}