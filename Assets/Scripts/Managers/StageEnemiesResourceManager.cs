using GM.Controllers;
using GM.Events;
using GM.UI.HUD;
using GM.Units;
using UnityEngine;

namespace GM.Managers
{
    public class StageEnemiesResourceManager : GM.Core.GMMonoBehaviour
    {
        [SerializeField] GameManager Manager;
        [Space]
        [SerializeField] CurrenciesDisplay GoldDiplay;

        void Awake()
        {
            Manager.E_OnEnemySpawn.AddListener(OnEnemySpawned);

            Manager.E_OnPreBossReady.AddListener(OnBossSpawned);
        }

        /* Callbacks */

        void OnEnemySpawned(GameObject enemy)
        {
            var health = enemy.GetComponent<HealthController>();

            health.E_OnZeroHealth.AddListener(() => OnEnemyDefeated(enemy));
        }

        void OnBossSpawned(StageBossEventPayload payload)
        {
            var health = payload.GameObject.GetComponent<HealthController>();

            health.E_OnZeroHealth.AddListener(() => OnBossDefeated(payload));
        }

        void OnBossDefeated(StageBossEventPayload payload)
        {
            var avatar = payload.GameObject.GetComponentInChildren<UnitAvatar>();

            BigDouble gold = App.Values.GoldPerStageBossAtStage(payload.StageSpawned);

            App.Inventory.Gold += gold;

            GoldDiplay.DisplayGoldTrail(avatar.Bounds.RandomPosition());
        }

        void OnEnemyDefeated(GameObject enemy)
        {
            var avatar = enemy.GetComponentInChildren<UnitAvatar>();

            BigDouble gold = App.Values.GoldPerEnemyAtStage(App.GameState.Stage);

            App.Inventory.Gold += gold;

            GoldDiplay.DisplayGoldTrail(avatar.Bounds.RandomPosition());
        }
    }
}
