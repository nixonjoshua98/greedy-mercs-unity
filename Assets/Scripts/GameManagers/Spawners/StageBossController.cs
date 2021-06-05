using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace GM
{
    using GM.Events;

    public class StageBossController : MonoBehaviour
    {
        public bool isAvoidingBoss { get; private set; } = false;

        [SerializeField] GameObject BossButton;

        [Header("Text")]
        [SerializeField] Text bossNameText;

        [Header("Objects")]
        [SerializeField] Transform BossSpawnPoint;

        [Header("Bosses")]
        [SerializeField] GameObject[] BossObjects;

        GameObject currentBossEnemy;

        public GameObjectEvent OnBossSpawn;

        void Awake()
        {
            OnBossSpawn = new GameObjectEvent();

            BossButton.SetActive(false);
        }

        public void Spawn()
        {
            isAvoidingBoss = false;

            bool isBountyBoss = StaticData.Bounty.IsBountyBoss(GameState.Stage.currentStage, out var boss);

            // Grab the boss which we are going to nstantiate later
            GameObject bossToSpawn = isBountyBoss ? StaticData.Bounty.Get(boss.ID).EnemyPrefab : BossObjects[Random.Range(0, BossObjects.Length)];

            // Set the name
            bossNameText.text = isBountyBoss ? boss.Name.ToUpper() : "BOSS BATTLE";

            // Instantiate the enemy object on the spawn location
            currentBossEnemy = Instantiate(bossToSpawn, BossSpawnPoint.position, Quaternion.identity, BossSpawnPoint);

            currentBossEnemy.GetComponent<Health>().OnDeath.AddListener(OnBossDeath);

            OnBossSpawn.Invoke(currentBossEnemy);
        }


        // = = = Button Callbacks ===
        public void OnSkipToBoss()
        {
            isAvoidingBoss = false;

            if (isAvoidingBoss)
                Spawn();

            BossButton.gameObject.SetActive(false);
        }

        void OnBossDeath(GameObject obj)
        {
            isAvoidingBoss = false;
        }

        public void OnTimerHitZero()
        {
            isAvoidingBoss = true;

            BossButton.SetActive(true);

            Destroy(currentBossEnemy);
        }
    }
}