using SRC.CameraControllers;
using SRC.Controllers;
using SRC.Events;
using SRC.Mercs;
using SRC.Units.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SRC
{
    public class GameManager : SRC.Core.GMMonoBehaviour
    {
        [SerializeField] private CameraController CamController;
        [SerializeField] private MercSquadController Mercs;

        [Header("References")]
        [HideInInspector] public List<GameObject> EnemyUnits;

        [Header("Prefabs")]
        [SerializeField] private GameObject EnemyObject;
        [SerializeField] private GameObject BossObject;

        [Header("Boss Events")]
        [HideInInspector] public UnityEvent<StageBossEventPayload> E_OnPreBossReady = new();
        [HideInInspector] public UnityEvent<StageBossEventPayload> E_OnBossReady = new();
        [HideInInspector] public UnityEvent E_OnBossDefeated = new();

        [HideInInspector] public UnityEvent<GameObject> E_OnPreEnemyReady = new();
        [HideInInspector] public UnityEvent E_OnEnemyDefeated = new();
        [HideInInspector] public UnityEvent<GameObject> E_OnEnemySpawn = new();

        //
        private Vector3 StageEnemyPosition = new(0, 8.5f);

        private GameState State { get => App.GameState; }

        private void Awake()
        {
            StageEnemyPosition.x = (CamController.Bounds.max.x - 2f);
        }

        private void Start()
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
            var stageBounty = App.Bounties.GetBountyForStage(App.GameState.Stage);

            GameObject unitToSpawn = stageBounty == null ? BossObject : stageBounty.Prefab;

            StageEnemyPosition += new Vector3(10, 0);

            GameObject bossObject = Instantiate(unitToSpawn, StageEnemyPosition, Quaternion.identity);

            // Setup Health
            HealthController health = bossObject.GetComponent<HealthController>();
            health.E_OnZeroHealth.AddListener(() => OnBossZeroHealth(bossObject));
            health.Init(App.Values.StageBossHealthAtStage(App.GameState.Stage));

            var eventPayload = new SRC.Events.StageBossEventPayload(go: bossObject,
                                                                    stage: State.Stage,
                                                                    isBounty: stageBounty != null);

            E_OnPreBossReady.Invoke(eventPayload);

            CamController.MoveCamera(new Vector3(10, 0), 1.75f);

            yield return Mercs.MoveUnitsToFormation(1.25f, 10);

            EnemyUnits.Add(bossObject);

            E_OnBossReady.Invoke(eventPayload);

            Mercs.SetControl(true);
        }

        private void OnEnemyZeroHealth(GameObject unitObject)
        {
            EnemyUnits.Remove(unitObject);

            App.GameState.EnemiesDefeated++;

            E_OnEnemyDefeated.Invoke();

            Continue();
        }

        private void OnBossZeroHealth(GameObject unitObject)
        {
            EnemyUnits.Remove(unitObject);

            App.GameState.Stage++;
            App.GameState.EnemiesDefeated = 0;

            E_OnBossDefeated.Invoke();

            Continue();
        }
    }
}