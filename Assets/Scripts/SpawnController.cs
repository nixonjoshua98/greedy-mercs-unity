using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using HealthController = GM.Controllers.HealthController;

namespace GM
{
    using GM.Units.Formations;

    public class SpawnController : Core.GMMonoBehaviour
    {
        [SerializeField] GameObject[] BossObjects;
        [SerializeField] GameObject[] EnemyObjects;
        [Space]
        [SerializeField] UnitFormation[] formations;


        [Header("Components - UI")]
        [SerializeField] Text bossTitleText;

        [SerializeField] GameObject bossEntryAnimationObject;

        void Awake()
        {
            bossEntryAnimationObject.SetActive(false);
        }


        public GameObject SpawnBoss(CurrentStageState state)
        {
            bool isBountyBoss = App.Data.Bounties.GetStageBounty(state.Stage, out var result);

            GameObject boss;

            if (isBountyBoss)
            {
                boss = SpawnBountyBoss(result);
            }

            else
            {
                boss = SpawnRegularBoss();
            }

            boss.GetComponent<HealthController>().OnZeroHealth.AddListener(() => { OnBossDeath(boss); });

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


        public List<GameObject> SpawnWave()
        {
            List<GameObject> spawnedObjects = new List<GameObject>();

            UnitFormation formation = formations[Random.Range(0, formations.Length)];

            Vector3 centerPos = GetWaveReferencePosition(formation);

            for (int i = 0; i < formation.NumPositions; ++i)
            {
                Vector3 spawnPos = centerPos + formation.GetPosition(i).ToVector3();

                GameObject spawnedEnemy = Instantiate(EnemyObjects[Random.Range(0, EnemyObjects.Length)], spawnPos, Quaternion.identity);

                spawnedObjects.Add(spawnedEnemy);
            }

            return spawnedObjects;
        }


        Vector3 GetWaveReferencePosition(UnitFormation chosenFormation)
        {
            Vector3 pos = Camera.main.MaxBounds();

            return new Vector3(pos.x + Mathf.Abs(chosenFormation.MinBounds().x) + 1.0f, 5.5f);
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