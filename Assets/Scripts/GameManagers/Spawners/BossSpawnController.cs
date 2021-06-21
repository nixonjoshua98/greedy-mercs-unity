using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GM
{

    public class BossSpawnController : MonoBehaviour
    {
        [SerializeField] GameObject[] BossObjects;

        [Header("Components - UI")]
        [SerializeField] Text bossTitleText;

        [SerializeField] GameObject bossEntryAnimationObject;

        private void Awake()
        {
            bossEntryAnimationObject.SetActive(false);
        }

        public GameObject Spawn()
        {
            CurrentStageState state = GameManager.Instance.GetState();

            // Check if Boss is a bounty target
            bool isBountyBoss = StaticData.Bounty.IsBountyBoss(state.currentStage, out var boss);

            // Grab the prefab for the target boss
            GameObject bossToSpawn = isBountyBoss ? StaticData.Bounty.Get(boss.ID).EnemyPrefab : BossObjects[Random.Range(0, BossObjects.Length)];

            // Update the title text
            bossTitleText.text = isBountyBoss ? boss.Name.ToUpper() : "BOSS BATTLE";

            // Instantiate the boss enemy
            GameObject o = Spawn(bossToSpawn, BossSpawnPosition());

            // Listen to Events
            o.GetComponent<AbstractHealthController>().E_OnDeath.AddListener(OnBossDeath);

            // Enable UI
            bossEntryAnimationObject.SetActive(true);

            return o;
        }

        GameObject Spawn(GameObject o, Vector3 pos)
        {
            return Instantiate(o, pos, Quaternion.identity);
        }

        Vector3 BossSpawnPosition()
        {
            List<Vector3> unitPositions = SquadManager.Instance.UnitPositions();

            Vector3 pos = new Vector3(3.6f, 6.5f);

            if (unitPositions.Count >= 1)
            {
                Vector3 averageUnitPosition = Funcs.AveragePosition(unitPositions);

                pos.x = averageUnitPosition.x + 2.0f;
            }

            return pos;
        }

        // = = = Events = = = //
        void OnBossDeath(GameObject o)
        {
            bossEntryAnimationObject.SetActive(false);
        }
    }
}