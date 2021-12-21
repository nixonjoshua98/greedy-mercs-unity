using GM.Targets;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using DamageClickController = GM.Controllers.DamageClickController;
using Random = UnityEngine.Random;

namespace GM
{
    public class CurrentStageState
    {
        public int Stage = 1;
        public int Wave = 1;
        public int WavesPerStage = 1;
    }

    public class GameManager : Core.GMMonoBehaviour
    {
        public static GameManager Instance = null;

        [Header("Controllers")]
        [SerializeField] MercSquadController squadController;
        [SerializeField] SpawnController spawner;
        [SerializeField] DamageClickController ClickController;

        [HideInInspector] public UnityEvent<UnitTarget> E_BossSpawn { get; private set; } = new UnityEvent<UnitTarget>();
        [HideInInspector] public UnityEvent<TargetList<UnitTarget>> E_OnWaveSpawn { get; private set; } = new UnityEvent<TargetList<UnitTarget>>();

        public CurrentStageState State { get; private set; } = new CurrentStageState();

        // Contains the wave enemies, but can also contain the stage-end boss
        // Enemies.TryGetWithType(TargetType.Boss) can be used to fetch the boss
        public TargetList<UnitTarget> Enemies { get; private set; } = new TargetList<UnitTarget>();
        public TargetList<MercUnitTarget> Mercs => squadController.Mercs;

        void Awake()
        {
            Instance = this;

            ClickController.E_OnClick.AddListener(OnDamageClick);
        }

        void Start()
        {
            SpawnWave();
        }

        public bool TryGetStageBoss(out UnitTarget boss)
        {
            boss = default;
            return Enemies.TryGetWithType(TargetType.Boss, ref boss);
        }

        void OnDamageClick(Vector2 worlPos)
        {
            if (Enemies.Count > 0)
            {
                BigDouble dmg = App.Cache.TotalTapDamage;

                UnitTarget target = Enemies.OrderBy(t => t.Health.Current).First();

                target.Health.TakeDamage(dmg);
            }
        }

        void SpawnWave()
        {
            Enemies.AddRange(spawner.SpawnWave().Select(x => new UnitTarget(x, TargetType.WaveEnemy)));

            BigDouble combinedHealth = App.Cache.EnemyHealthAtStage(State.Stage);

            foreach (UnitTarget trgt in Enemies)
            {
                trgt.Health.Init(val: combinedHealth / Enemies.Count);

                trgt.Health.OnZeroHealth.AddListener(() => OnEnemyZeroHealth(trgt));
            }

            E_OnWaveSpawn.Invoke(Enemies);
        }

        UnitTarget SpawnBoss()
        {
            UnitTarget boss = new UnitTarget(spawner.SpawnBoss(State), TargetType.Boss);

            boss.Health.Init(val: App.Cache.StageBossHealthAtStage(State.Stage));

            boss.Health.OnZeroHealth.AddListener(OnBossZeroHealth);

            Enemies.Add(boss);

            E_BossSpawn.Invoke(boss);

            return boss;
        }

        void OnEnemyZeroHealth(UnitTarget trgt)
        {
            Enemies.Remove(trgt);

            if (Enemies.Count == 0)
            {
                OnWaveCleared();
            }
        }

        void OnBossZeroHealth()
        {
            Enemies.Clear();

            State.Stage++;
            State.Wave = 1;

            SpawnWave();
        }

        void OnWaveCleared()
        {
            if (State.Wave == State.WavesPerStage)
            {
                StartCoroutine(SetupBossFight());
            }

            else
            {
                SpawnWave();

                State.Wave++;
            }
        }

        IEnumerator SetupBossFight()
        {
            bool isBossReady = false;
            bool isMercsReady = false;

            UnitTarget boss = SpawnBoss();

            List<Vector2> formationPositions = new List<Vector2>();

            if (squadController.Mercs.Count > 0)
            {
                // Start to move mercs to the formation positions
                formationPositions = squadController.MoveMercsToStageBossFormation(boss, () => isMercsReady = true);
            }
            else
            {
                GMLogger.Editor("TODO");
            }

            // Grab the left most merc in the formation which we will use to calculate the boss position
            Vector2 leftMostPosition = formationPositions.OrderBy(pos => pos.x).Last();

            // Set the boss position off-screen
            boss.GameObject.transform.position = new Vector3(Camera.main.MaxBounds().x + 2.5f, formationPositions.Min(pos => pos.y));

            // Move the boss towards it's postion
            boss.Controller.MoveTowards(boss.GameObject.transform.position - new Vector3(2.5f, 0), () =>
            {
                isBossReady = true; // Set the flag

                // Default back to the idle animation
                boss.Avatar.PlayAnimation(boss.Avatar.AnimationStrings.Idle);
            });

            // Pause here until the boss is ready and the mercs are in formation
            yield return new WaitUntil(() => isBossReady && isMercsReady);

            // Pre-set the priority target to avoid target issues with the delay below
            squadController.Mercs.ForEach(merc => merc.Controller.SetPriorityTarget(boss));

            foreach (var merc in squadController.Mercs)
            {
                // Purely to make sure the animations are not all synced up
                yield return new WaitForSecondsRealtime(Random.Range(0, 0.2f));

                // Give back control to the controller
                merc.Controller.Resume();
            }
        }
    }
}