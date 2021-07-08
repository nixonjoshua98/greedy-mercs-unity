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
            CurrentStageState state = GameManager.Instance.State();

            // Check if Boss is a bounty target
            bool isBountyBoss = StaticData.Bounty.IsBountyBoss(state.currentStage, out var boss);

            // Grab the prefab for the target boss
            GameObject bossToSpawn = isBountyBoss ? StaticData.Bounty.Get(boss.ID).EnemyPrefab : BossObjects[Random.Range(0, BossObjects.Length)];

            // Update the title text
            bossTitleText.text = isBountyBoss ? boss.Name.ToUpper() : "BOSS BATTLE";

            // Instantiate the boss enemy
            GameObject o = Spawn(bossToSpawn, SpawnPosition());

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

        Vector3 SpawnPosition()
        {
            List<Vector3> positions = SquadManager.Instance.UnitPositions();

            if (positions.Count == 0)
                return new Vector3(0.0f, 5.5f);

            Vector3 centerPos = Funcs.AveragePosition(positions);

            return new Vector3(centerPos.x + 5.0f, 5.5f);
        }

        // = = = Events = = = //

        void OnBossDeath(GameObject o)
        {
            bossEntryAnimationObject.SetActive(false);
        }
    }
}