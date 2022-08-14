using GM.Bounties.Models;
using GM.CameraControllers;
using GM.Controllers;
using GM.Events;
using GM.Mercs;
using GM.Units.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GM
{
    public class GameManager : GM.Core.GMMonoBehaviour
    {
        [SerializeField] CameraController CamController;
        [SerializeField] MercSquadController Mercs;

        [Header("References")]
        [HideInInspector] public List<GameObject> EnemyUnits;

        [Header("Prefabs")]
        [SerializeField] GameObject EnemyObject;
        [SerializeField] GameObject BossObject;

        [Header("Boss Events")]
        [HideInInspector] public UnityEvent<StageBossEventPayload> E_OnPreBossReady = new();
        [HideInInspector] public UnityEvent<StageBossEventPayload> E_OnBossReady = new();
        [HideInInspector] public UnityEvent E_OnBossDefeated = new();

        [HideInInspector] public UnityEvent<GameObject> E_OnPreEnemyReady = new();
        [HideInInspector] public UnityEvent E_OnEnemyDefeated = new();
        [HideInInspector] public UnityEvent<GameObject> E_OnEnemySpawn = new();

        //
        Vector3 StageEnemyPosition = new(0, 8.5f);

        GameState State { get => App.GameState; }

        void Awake()
        {
            StageEnemyPosition.x = (CamController.Bounds.max.x - 2f);
        }

        void Start()
        {
            Continue();
        }

        public void Continue()
        {
            if (App.GameState.EnemiesRemaining == 0)
            {
                this.InvokeAfter(0.25f, SetupStageBoss);
            }
            else
            {
                this.InvokeAfter(0.25f, SetupStageEnemy);
            }
        }

        private IEnumerator SetupStageEnemy()
        {
            BigDouble maxHealth = App.Values.EnemyHealthAtStage(App.GameState.Stage);

            var enemySpawnPos = StageEnemyPosition + new Vector3(5, 0);

            GameObject enemy = Instantiate(EnemyObject, enemySpawnPos, Quaternion.identity);

            HealthController health = enemy.GetComponent<HealthController>();

            health.E_OnZeroHealth.AddListener(() => OnEnemyZeroHealth(enemy));

            health.Init(maxHealth);

            var controller = enemy.GetComponent<EnemyUnitController>();

            E_OnPreEnemyReady.Invoke(enemy);

            yield return controller.Movement.MoveToPositionEnumerator(StageEnemyPosition);

            EnemyUnits.Add(enemy);

            E_OnEnemySpawn.Invoke(enemy);
        }

        private IEnumerator SetupStageBoss()
        {
            GameObject unitToSpawn = App.Bounties.TryGetStageBounty(App.GameState.Stage, out Bounty bountyData) ? bountyData.Prefab : BossObject;

            StageEnemyPosition += new Vector3(10, 0);

            GameObject bossObject = Instantiate(unitToSpawn, StageEnemyPosition, Quaternion.identity);

            // Setup Health
            HealthController health = bossObject.GetComponent<HealthController>();
            health.E_OnZeroHealth.AddListener(() => OnBossZeroHealth(bossObject));
            health.Init(App.Values.StageBossHealthAtStage(App.GameState.Stage));

            var eventPayload = new GM.Events.StageBossEventPayload(bossObject, State.Stage);

            E_OnPreBossReady.Invoke(eventPayload);

            CamController.MoveCamera(new Vector3(10, 0), 1.75f);

            yield return Mercs.MoveUnitsToFormation(1.25f, 10);

            EnemyUnits.Add(bossObject);

            E_OnBossReady.Invoke(eventPayload);

            Mercs.SetControl(true);
        }

        void OnEnemyZeroHealth(GameObject unitObject)
        {
            EnemyUnits.Remove(unitObject);

            App.GameState.EnemiesDefeated++;

            E_OnEnemyDefeated.Invoke();

            Continue();
        }

        void OnBossZeroHealth(GameObject unitObject)
        {
            EnemyUnits.Remove(unitObject);

            App.GameState.Stage++;
            App.GameState.EnemiesDefeated = 0;

            E_OnBossDefeated.Invoke();

            Continue();
        }
    }
}