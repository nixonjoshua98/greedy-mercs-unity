using GM.Bounties.Models;
using GM.Common;
using GM.Controllers;
using GM.Units;
using UnityEngine;
using UnityEngine.Events;

namespace GM
{
    public class GameManager : GM.Core.GMMonoBehaviour
    {
        [Header("References")]
        [SerializeField] UnitCollection EnemyUnits;

        [Header("Prefabs")]
        public GameObject EnemyObject;
        public GameObject BossObject;

        [Header("Events")]
        [HideInInspector] public UnityEvent<SpawnedBoss> E_BossSpawn = new();
        [HideInInspector] public UnityEvent E_BossDefeated = new();
        [HideInInspector] public UnityEvent E_EnemyDefeated = new();

        public void Start()
        {
            if (App.GameState.EnemiesRemaining == 0)
            {
                SpawnBoss();
            }
            else
            {
                StartWave();
            }
        }

        private void StartWave()
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

        private void SpawnBoss()
        {
            SpawnedBoss enemy = InstantiateEnemyBossUnit();

            // Components
            HealthController health = enemy.GameObject.GetComponent<HealthController>();

            // Setup
            health.Init(App.GMCache.StageBossHealthAtStage(App.GameState.Stage));

            // Set the boss position off-screen
            enemy.GameObject.transform.position = new Vector3(Camera.main.Bounds().max.x + 2.5f, Constants.CENTER_BATTLE_Y);

            E_BossSpawn.Invoke(enemy);
        }


        public GameObject InstantiateEnemyUnit()
        {
            GameObject obj = Instantiate(EnemyObject, EnemyUnitSpawnPosition(), Quaternion.identity);

            UnitBase unit = obj.GetComponent<UnitBase>();
            AbstractHealthController health = obj.GetComponent<AbstractHealthController>();

            health.Invincible = true;

            health.E_OnZeroHealth.AddListener(() => OnEnemyZeroHealth(obj));

            EnemyUnits.Add(unit);

            // Unit cannot be attacked until they are visible on screen
            Enumerators.InvokeAfter(this, () => Camera.main.IsVisible(unit.Avatar.Bounds.min), () => health.Invincible = false);

            return obj;
        }


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

            EnemyUnits.Add(unit);

            // Unit cannot be attacked until they are visible on screen
            Enumerators.InvokeAfter(this, () => Camera.main.IsVisible(bossObject.transform.position), () => health.Invincible = false);

            return new() { GameObject = bossObject, BountyID = bountyData?.ID };
        }


        private Vector3 EnemyUnitSpawnPosition()
        {
            if (EnemyUnits.Count > 0)
            {
                UnitBase unit = EnemyUnits.Last();

                return new Vector3(unit.Avatar.Bounds.max.x + (unit.Avatar.Bounds.size.x / 2) + 1.0f, unit.transform.position.y);
            }

            return new Vector3(8, GM.Common.Constants.CENTER_BATTLE_Y) + new Vector3(Camera.main.Bounds().max.x, 0);
        }

        void OnEnemyZeroHealth(GameObject unitObject)
        {
            UnitBase unit = unitObject.GetComponent<UnitBase>();

            EnemyUnits.Remove(unit);

            App.GameState.EnemiesDefeated++;

            E_EnemyDefeated.Invoke();

            if (EnemyUnits.Count == 0)
            {
                SpawnBoss();
            }
        }

        void OnBossZeroHealth(GameObject unitObject)
        {
            UnitBase unit = unitObject.GetComponent<UnitBase>();

            EnemyUnits.Remove(unit);

            App.GameState.Stage++;
            App.GameState.EnemiesDefeated = 0;

            E_BossDefeated.Invoke();

            StartWave();
        }
    }
}