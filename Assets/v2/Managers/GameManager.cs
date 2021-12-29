using GM.Mercs;
using GM.Targets;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace GM
{
    public class GameManager : Core.GMMonoBehaviour
    {
        public static GameManager Instance = null;

        [Header("Controllers")]
        [SerializeField] SpawnController spawner;
        public MercSquadController MercSquad;

        [HideInInspector] public UnityEvent<UnitTarget> E_BossSpawn { get; private set; } = new UnityEvent<UnitTarget>();
        [HideInInspector] public UnityEvent<TargetList<UnitTarget>> E_OnWaveSpawn { get; private set; } = new UnityEvent<TargetList<UnitTarget>>();

        public TargetList<UnitTarget> Enemies { get; private set; } = new TargetList<UnitTarget>();

        // State
        public bool IsBossFight { get; private set; }

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            MercSquad.OnUnitAddedToSquad.AddListener(OnUnitAddedToSquad);

            SetupWave();
        }

        public bool TryGetEnemy(out UnitTarget target)
        {
            target = default;

            if (Enemies.Count > 0)
            {
                target = Enemies[0];
                return true;
            }
            return false;
        }

        bool TryGetBoss(out UnitTarget trgt)
        {
            trgt = default;
            return Enemies.TryGetWithType(TargetType.Boss, ref trgt);
        }

        UnitTarget SpawnBoss()
        {
            UnitTarget boss = new UnitTarget(spawner.SpawnBoss(), TargetType.Boss);

            boss.Health.Init(val: App.Cache.StageBossHealthAtStage(App.Data.GameState.Stage));

            boss.Health.OnZeroHealth.AddListener(OnBossZeroHealth);

            Enemies.Add(boss);

            InvokeBossSpawnEvent(boss);

            return boss;
        }

        void OnWaveCleared()
        {
            if (App.Data.GameState.Wave == App.Data.GameState.WavesPerStage)
            {
                StartCoroutine(SetupBossFight());
            }

            else
            {
                SetupWave();

                App.Data.GameState.Wave++;
            }
        }

        void SetupWave()
        {
            Enemies.AddRange(spawner.SpawnWave().Select(x => new UnitTarget(x, TargetType.WaveEnemy)));

            BigDouble combinedHealth = App.Cache.EnemyHealthAtStage(App.Data.GameState.Stage);

            foreach (UnitTarget trgt in Enemies)
            {
                trgt.Health.Invulnerable = true;

                trgt.Health.Init(val: combinedHealth / Enemies.Count);

                trgt.Health.OnZeroHealth.AddListener(() => OnEnemyZeroHealth(trgt));

                StartCoroutine(Enumerators.InvokeAfter(() => Camera.main.IsVisible(trgt.Position), () => trgt.Health.Invulnerable = false));
            }

            InvokeWaveSpawnEvent();
        }

        IEnumerator SetupBossFight()
        {
            bool isBossReady = false;
            bool isMercsReady = false;

            UnitTarget boss = SpawnBoss();

            boss.Health.Invulnerable = true;

            float yBossPosition;

            if (MercSquad.Mercs.Count > 0)
            {
                MercSquad.MoveMercsToFormation((merc) => { merc.Controller.LookAt(boss.Position); }, () => { isMercsReady = true; GMLogger.Editor("Mercs in formation"); });
            }
            else
            {
                // Set 'isMercsReady' after the boss is finished setting up
                StartCoroutine(Enumerators.InvokeAfter(() => isBossReady, () => isMercsReady = true));
            }

            yBossPosition = GM.Common.Constants.CENTER_BATTLE_Y;

            // Set the boss position off-screen
            boss.GameObject.transform.position = new Vector3(Camera.main.MaxBounds().x + 2.5f, yBossPosition);

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
            MercSquad.Mercs.ForEach(merc => merc.Controller.SetPriorityTarget(boss));

            boss.Health.Invulnerable = false; // Allow damage

            // Give back control to the controller
            MercSquad.Mercs.ForEach(merc => merc.Controller.Resume());
        }

        // = Event Invocations = //

        void InvokeBossSpawnEvent(UnitTarget boss)
        {
            OnBossSpawn();
            E_BossSpawn.Invoke(boss);
        }

        void InvokeWaveSpawnEvent()
        {
            E_OnWaveSpawn.Invoke(Enemies);
        }

        // = Event Callbacks = //

        void OnUnitAddedToSquad(SquadMerc merc)
        {
            if (IsBossFight && !MercSquad.IsMovingToFormation)
            {
                MercSquad.MoveMercToFormationPosition(merc.MercId);

                if (TryGetBoss(out UnitTarget boss))
                {
                    merc.Controller.SetPriorityTarget(boss);
                }
            }
        }

        void OnEnemyZeroHealth(UnitTarget trgt)
        {
            Enemies.Remove(trgt);

            if (Enemies.Count == 0)
            {
                OnWaveCleared();
            }
        }

        void OnBossSpawn()
        {
            IsBossFight = true;
        }

        void OnBossZeroHealth()
        {
            Enemies.Clear();

            IsBossFight = false;

            App.Data.GameState.Stage++;
            App.Data.GameState.Wave = 1;

            SetupWave();
        }
    }
}