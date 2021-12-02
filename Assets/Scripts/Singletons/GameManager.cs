﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GM
{
    using GM.Events;

    public class CurrentStageState
    {
        public int Stage = 1;
        public int Wave = 1;
        public int WavesPerStage = 1;

        [System.NonSerialized]
        public bool IsBossAvailable = false;

        public CurrentStageState Copy()
        {
            return new CurrentStageState()
            {
                Stage = Stage,
                Wave = Wave,
                WavesPerStage = WavesPerStage
            };
        }
    }

    public class GameManager : Core.GMMonoBehaviour
    {
        public static GameManager Instance = null;

        [Header("Controllers")]
        [SerializeField] SpawnController spawner;

        public CurrentStageState state;

        GameObject currentStageBoss;

        // Events
        public UnityEvent<GameObject> E_BossSpawn = new UnityEvent<GameObject>();
        public UnityEvent E_BossDeath = new UnityEvent();
        [HideInInspector] public UnityEvent E_OnWaveCleared;
        [HideInInspector] public UnityEvent<WaveSpawnEventData> E_OnWaveSpawn;

        List<GameObject> waveEnemies = new List<GameObject>();

        public DamageClickController ClickController;

        void Awake()
        {
            Instance = this;

            state = new CurrentStageState();

            ClickController.E_OnClick.AddListener(OnDamageClick);
        }


        void Start()
        {
            SpawnWave();
        }


        void OnDamageClick(Vector2 worldSpaceClickPosition)
        {
            if (waveEnemies.Count >= 1)
            {
                GameObject target = waveEnemies[0];

                Debug.Log($"Click attacked {target.name} {worldSpaceClickPosition}");
            }
        }

        public CurrentStageState State() => state.Copy();

        public bool TryGetBoss(out GameObject boss)
        {
            boss = currentStageBoss;

            return state.IsBossAvailable;
        }

        // = = = ^ //


        void SpawnWave()
        {
            waveEnemies = spawner.SpawnWave();

            BigDouble combinedHealth = App.Cache.EnemyHealthAtStage(state.Stage);

            List<HealthController> healthControllers = new List<HealthController>();

            foreach (GameObject o in waveEnemies)
            {
                HealthController hp = o.GetComponent<HealthController>();

                hp.Setup(combinedHealth / waveEnemies.Count);

                hp.E_OnZeroHealth.AddListener(() => {
                    OnEnemyZeroHealth(o);
                });

                healthControllers.Add(hp);
            }

            E_OnWaveSpawn.Invoke(new WaveSpawnEventData()
            {
                CombinedHealth = combinedHealth,
                HealthControllers = healthControllers
            });
        }


        void SpawnBoss()
        {
            // Spawn the object
            currentStageBoss = spawner.SpawnBoss(state);

            // Grab components
            HealthController hp = currentStageBoss.GetComponent<HealthController>();

            hp.Setup(val: App.Cache.StageBossHealthAtStage(state.Stage));

            hp.E_OnZeroHealth.AddListener(OnBossZeroHealth);

            OnBossSpawn();

            E_BossSpawn.Invoke(currentStageBoss);
        }


        // = = = Events = = = //
        void OnBossSpawn()
        {
            state.IsBossAvailable = true;
        }

        /// <summary>
        /// Called once a wave enemy has their health reduced to 0
        /// </summary>
        void OnEnemyZeroHealth(GameObject obj)
        {
            waveEnemies.Remove(obj);

            if (waveEnemies.Count == 0)
            {
                OnWaveCleared();
            }
        }

        void OnBossZeroHealth()
        {
            E_BossDeath.Invoke();
            currentStageBoss = null;

            state.Stage++;
            state.Wave = 1;
            state.IsBossAvailable = false;

            SpawnWave();
        }


        void OnWaveCleared()
        {
            E_OnWaveCleared.Invoke();

            if (state.Wave == state.WavesPerStage)
                SpawnBoss();

            else
            {
                SpawnWave();

                state.Wave++;
            }
        }
        // = = = ^ //
    }
}