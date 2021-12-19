﻿using GM.Mercs.Controllers;
using GM.Targets;
using GM.Units.Formations;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using DamageClickController = GM.Controllers.DamageClickController;
using MercID = GM.Common.Enums.MercID;

namespace GM
{
    public class SquadMerc: Target
    {
        public IMercController Controller { get; private set; }
        public IMovementController Movement { get; private set; }

        public SquadMerc (GameObject obj) : base(obj, TargetType.Unset)
        {
            Controller = obj.GetComponent<IMercController>();
            Movement = obj.GetComponent<IMovementController>();
        }
    }

    public class CurrentStageState
    {
        public int Stage = 1;
        public int Wave = 1;
        public int WavesPerStage = 1;
    }

    public class GameManager : Core.GMMonoBehaviour
    {
        [SerializeField] UnitFormation formation;

        public static GameManager Instance = null;

        [Header("Controllers")]
        [SerializeField] SpawnController spawner;
        [SerializeField] DamageClickController ClickController;

        [HideInInspector] public UnityEvent<Target> E_BossSpawn { get; private set; } = new UnityEvent<Target>();
        [HideInInspector] public UnityEvent<TargetList<Target>> E_OnWaveSpawn { get; private set; } = new UnityEvent<TargetList<Target>>();

        public CurrentStageState State { get; private set; } = new CurrentStageState();

        // Contains the wave enemies, but can also contain the stage-end boss
        // Enemies.TryGetWithType(TargetType.Boss) can be used to fetch the boss
        public TargetList<Target> Enemies { get; private set; } = new TargetList<Target>();
        public TargetList<SquadMerc> Mercs { get; private set; } = new TargetList<SquadMerc>();

        public List<Vector3> UnitPositions => Mercs.Where(obj => obj.GameObject != null).Select(obj => obj.Position).ToList();

        void Awake()
        {
            Instance = this;

            ClickController.E_OnClick.AddListener(OnDamageClick);

            App.Data.Mercs.E_MercUnlocked.AddListener((merc) =>
            {
                Vector2 pos = new Vector2(Camera.main.MinBounds().x, Common.Constants.CENTER_BATTLE_Y);

                if (Mercs.Count < 2)
                {
                    AddMercToSquad(merc, pos);
                }
            });
        }

        void Start()
        {
            SpawnWave();
        }

        void OnDamageClick(Vector2 worldSpaceClickPosition)
        {
            if (Enemies.Count > 0)
            {
                Target target = Enemies.OrderBy(t => t.Health.Current).First();

                target.Health.TakeDamage(App.Cache.TotalTapDamage);
            }
        }

        void SpawnWave()
        {
            Enemies.AddRange(spawner.SpawnWave().Select(x => new Target(x, TargetType.WaveEnemy)));

            BigDouble combinedHealth = App.Cache.EnemyHealthAtStage(State.Stage);

            foreach (Target trgt in Enemies)
            {
                trgt.Health.Init(val: combinedHealth / Enemies.Count);

                trgt.Health.E_OnZeroHealth.AddListener(() => OnEnemyZeroHealth(trgt));
            }

            E_OnWaveSpawn.Invoke(Enemies);
        }

        Target SpawnBoss()
        {
            Target boss = new Target(spawner.SpawnBoss(State), TargetType.Boss);

            boss.Health.Init(val: App.Cache.StageBossHealthAtStage(State.Stage));

            boss.Health.E_OnZeroHealth.AddListener(OnBossZeroHealth);

            Enemies.Add(boss);

            E_BossSpawn.Invoke(boss);

            return boss;
        }

        void AddMercToSquad(MercID merc, Vector2 pos)
        {
            Mercs.Models.MercGameDataModel data = App.Data.Mercs.GetGameMerc(merc);

            GameObject o = Instantiate(data.Prefab, pos, Quaternion.identity);

            Mercs.Add(new SquadMerc(o));
        }

        /// <summary> Take control of each merc and move them into formation </summary>
        void MoveMercsToFormation(Action<SquadMerc> mercCallback, Action finalCallback)
        {
            Vector3 cameraPosition = Camera.main.MinBounds();

            float offsetX = Mathf.Abs(formation.MinBounds().x) + 1.0f;

            int mercsMoved = 0;

            foreach ((SquadMerc merc, int idx) in Mercs.Select((merc, idx) => (merc, idx)))
            {
                merc.Controller.Pause(); // Pause the regular merc update loop

                Vector2 relPos = formation.GetPosition(Mathf.Min(formation.NumPositions - 1, idx));

                Vector2 targetPosition = new Vector2(offsetX + cameraPosition.x + relPos.x, relPos.y + Common.Constants.CENTER_BATTLE_Y);

                merc.Controller.MoveTowards(targetPosition, () =>
                {
                    mercsMoved++;

                    mercCallback.Invoke(merc);

                    if (mercsMoved == Mercs.Count)
                    {
                        finalCallback();
                    }
                });
            }
        }

        void SetupStageBossFight(Target boss)
        {
            MoveMercsToFormation(

                // Once the merc reaches their spot, then face towards the boss
                (merc) => merc.Movement.FaceTowards(boss.GameObject),

                // Set the boss target and resume normal control
                () => {
                    Mercs.ForEach(m => {
                        m.Controller.SetPriorityTarget(boss);                        
                        m.Controller.Resume();
                    });
                });
        }

        void OnEnemyZeroHealth(Target trgt)
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
                Target boss = SpawnBoss();

                SetupStageBossFight(boss);
            }

            else
            {
                SpawnWave();

                State.Wave++;
            }
        }
    }
}