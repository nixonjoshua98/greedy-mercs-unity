using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using GM;
using GM.Events;

namespace GreedyMercs
{
    using GM.Bounty;

    public class BossBattleManager : MonoBehaviour
    {
        public static BossBattleManager Instance = null;

        public bool isAvoidingBoss { get; private set; } = false;

        [Header("UI Objects & Components")]
        [SerializeField] GameObject bossAnimObject;

        [SerializeField] Slider BossSlider;

        [SerializeField] GameObject BossButton;

        [Header("Text")]
        [SerializeField] Text BossTimerText;
        [SerializeField] Text bossNameText;

        [Header("Objects")]
        [SerializeField] Transform BossSpawnPoint;

        [Header("Bosses")]
        [SerializeField] GameObject[] BossObjects;

        GameObject CurrentBossEnemy;

        // Events
        public UnityEvent OnBossDeath;
        public UnityEvent OnFailedToKillBoss;
        public GameObjectEvent OnBossSpawn;


        void Awake()
        {
            Instance = this;

            OnBossDeath = new UnityEvent();
            OnBossSpawn = new GameObjectEvent();
            OnFailedToKillBoss = new UnityEvent();

            SetUIActive(false);

            BossButton.SetActive(false);
        }

        void SetUIActive(bool active)
        {
            bossAnimObject.SetActive(active);

            BossSlider.gameObject.SetActive(active);
            BossTimerText.gameObject.SetActive(active);
        }

        public static void StartBossBattle()
        {
            Instance.StartCoroutine(Instance.IBossBattle());
        }



        // = = = Button Callbacks ===
        public void OnFightBossButton()
        {
            isAvoidingBoss = false;

            if (!isAvoidingBoss && GameManager.Instance.IsAllStageEnemiesKilled)
            {
                StartBossBattle();
            }

            BossButton.gameObject.SetActive(false);
        }

        IEnumerator IBossBattle()
        {
            PrepareBossBattle();

            yield return BossBattleLoop();

            OnAfterBossBattle();
        }

        void PrepareBossBattle()
        {
            SetUIActive(true);

            bool isBountyBoss = StaticData.Bounty.IsBountyBoss(GameState.Stage.stage, out var boss);

            // Grab the boss which we are going to nstantiate later
            GameObject bossToSpawn = isBountyBoss ? StaticData.Bounty.Get(boss.ID).prefab : BossObjects[Random.Range(0, BossObjects.Length)];

            // Set the name
            bossNameText.text = isBountyBoss ? boss.Name.ToUpper() : "BOSS BATTLE";

            // Instantiate the enemy object on the spawn location
            CurrentBossEnemy = Instantiate(bossToSpawn, BossSpawnPoint.position, Quaternion.identity, BossSpawnPoint);

            OnBossSpawn.Invoke(CurrentBossEnemy);
        }

        IEnumerator BossBattleLoop()
        {
            float maxTimer = StatsCache.StageEnemy.BossTimer;

            float timer = maxTimer;

            BossSlider.value = timer;

            while (CurrentBossEnemy != null && timer > 0)
            {
                timer -= Time.deltaTime;

                BossSlider.value = timer / maxTimer;

                BossTimerText.text = Mathf.CeilToInt(timer).ToString();

                yield return new WaitForEndOfFrame();
            }
        }

        void OnAfterBossBattle()
        {
            SetUIActive(false);

            if (CurrentBossEnemy != null)
            {
                BossButton.SetActive(true);

                isAvoidingBoss = true;

                Destroy(CurrentBossEnemy);

                OnFailedToKillBoss.Invoke();
            }

            else
            {
                OnBossDeath.Invoke();
            }
        }

    }
}