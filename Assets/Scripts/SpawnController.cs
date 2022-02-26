
using UnityEngine;
using UnityEngine.UI;
using HealthController = GM.Controllers.HealthController;

namespace GM
{
    public class SpawnController : Core.GMMonoBehaviour
    {
        [SerializeField] GameObject[] BossObjects;


        [Header("Components - UI")]
        [SerializeField] Text bossTitleText;

        [SerializeField] GameObject bossEntryAnimationObject;

        void Awake()
        {
            bossEntryAnimationObject.SetActive(false);
        }


        public GameObject SpawnBoss()
        {
            bool isBountyBoss = App.Data.Bounties.GetStageBounty(App.Data.GameState.Stage, out var result);

            GameObject boss;

            if (isBountyBoss)
            {
                boss = SpawnBountyBoss(result);
            }

            else
            {
                boss = SpawnRegularBoss();
            }

            boss.GetComponent<HealthController>().E_OnZeroHealth.AddListener(() => { OnBossDeath(boss); });

            return boss;
        }


        GameObject SpawnRegularBoss()
        {
            GameObject bossToSpawn = BossObjects[Random.Range(0, BossObjects.Length)];
            GameObject spawnedBoss = Instantiate(bossToSpawn);

            OnBossSpawn(spawnedBoss);

            return spawnedBoss;
        }


        GameObject SpawnBountyBoss(Bounties.Models.BountyGameData bounty)
        {
            GameObject spawnedBoss = Instantiate(bounty.Prefab);

            OnBossSpawn(spawnedBoss, bounty);

            return spawnedBoss;
        }


        // = = = Events = = = //
        void OnBossDeath(GameObject boss)
        {
            bossEntryAnimationObject.SetActive(false);
        }


        void OnBossSpawn(GameObject boss)
        {
            bossTitleText.text = "BOSS";

            bossEntryAnimationObject.SetActive(true);
        }


        void OnBossSpawn(GameObject boss, Bounties.Models.BountyGameData bounty)
        {
            bossTitleText.text = bounty.Name;

            bossEntryAnimationObject.SetActive(true);
        }
    }
}