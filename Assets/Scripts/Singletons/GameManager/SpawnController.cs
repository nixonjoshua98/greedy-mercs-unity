using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;


namespace GM
{
    using GM.Data;
    using GM.Units.Formations;
    using GM.Bounties;

    public class SpawnController : MonoBehaviour
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
            bool isBountyBoss = GameData.Get.Bounties.GetStageBounty(state.Stage, out BountyData result);

            if (isBountyBoss && MathUtils.PercentChance(result.SpawnChance))
                return SpawnBountyBoss(result);

            else
                return SpawnRegularBoss();
        }


        GameObject SpawnRegularBoss()
        {
            GameObject bossToSpawn = BossObjects[Random.Range(0, BossObjects.Length)];
            GameObject spawnedBoss = Instantiate(bossToSpawn, GetBossSpawnPosition(), Quaternion.identity);

            spawnedBoss.GetComponent<HealthController>().E_OnZeroHealth.AddListener(() => {
                OnBossDeath(spawnedBoss);
            });

            OnBossSpawn(spawnedBoss);

            return spawnedBoss;
        }


        GameObject SpawnBountyBoss(BountyData bounty)
        {
            GameObject spawnedBoss = Instantiate(bounty.Prefab, GetBossSpawnPosition(), Quaternion.identity);

            spawnedBoss.GetComponent<HealthController>().E_OnZeroHealth.AddListener(() => {
                OnBossDeath(spawnedBoss);
            });

            OnBossSpawn(spawnedBoss, bounty);

            return spawnedBoss;
        }


        public List<GameObject> SpawnWave()
        {
            List<GameObject> spawnedObjects = new List<GameObject>();

            UnitFormation formation = formations[Random.Range(0, formations.Length)];

            Vector3 centerPos = GetWaveReferencePosition(formation);

            for (int i = 0; i < formation.numPositions; ++i)
            {
                Vector3 spawnPos = centerPos + formation.GetPosition(i).ToVector3();

                GameObject spawnedEnemy = Instantiate(EnemyObjects[Random.Range(0, EnemyObjects.Length)], spawnPos, Quaternion.identity);

                spawnedObjects.Add(spawnedEnemy);
            }

            return spawnedObjects;
        }


        Vector3 GetBossSpawnPosition()
        {
            Vector3 pos = Camera.main.MaxBounds();

            return new Vector3(pos.x - 2.0f, Constants.CENTER_BATTLE_Y);
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


        void OnBossSpawn(GameObject boss, BountyData bounty)
        {
            bossTitleText.text = bounty.Name;

            bossEntryAnimationObject.SetActive(true);
        }
    }
}