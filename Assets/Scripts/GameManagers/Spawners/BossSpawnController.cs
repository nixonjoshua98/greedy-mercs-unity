using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GM
{
    using GM.Data;
    using GM.Bounty;

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

            GameObject bossToSpawn;

            bossTitleText.text = "BOSS";

            if (GameData.Get().Bounties.GetStageBounty(state.currentStage, out BountyData result))
            {
                // Grab the prefab for the target boss
                bossToSpawn = result.Prefab;

                // Update the title text
                bossTitleText.text = result.Name.ToUpper();
            }

            else
            {
                // Grab the prefab for the target boss
                bossToSpawn = BossObjects[Random.Range(0, BossObjects.Length)];
            }

            // Instantiate the boss enemy
            GameObject o = Spawn(bossToSpawn, SpawnPosition());

            // Listen to Events
            o.GetComponent<HealthController>().E_OnDeath.AddListener(() => { OnBossDeath(o); });

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