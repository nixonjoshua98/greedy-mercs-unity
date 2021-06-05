using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GM
{
    using GM.Events;
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
            C_GameState state = GameManager.Instance.GetState();

            // Check if Boss is a bounty target
            bool isBountyBoss = StaticData.Bounty.IsBountyBoss(state.currentStage, out var boss);

            // Grab the prefab for the target boss
            GameObject bossToSpawn = isBountyBoss ? StaticData.Bounty.Get(boss.ID).EnemyPrefab : BossObjects[Random.Range(0, BossObjects.Length)];

            // Update the title text
            bossTitleText.text = isBountyBoss ? boss.Name.ToUpper() : "BOSS BATTLE";

            // Instantiate the boss enemy
            GameObject o = Spawn(bossToSpawn, new Vector3(3.6f, 2.3f));

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

        // = = = Events = = = //
        void OnBossDeath(GameObject o)
        {
            bossEntryAnimationObject.SetActive(false);
        }
    }
}