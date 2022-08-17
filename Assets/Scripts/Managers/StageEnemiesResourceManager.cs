using SRC.Controllers;
using SRC.Events;
using SRC.UI.HUD;
using SRC.Units;
using UnityEngine;

namespace SRC.Managers
{
    public class StageEnemiesResourceManager : SRC.Core.GMMonoBehaviour
    {
        [SerializeField] private GameManager Manager;
        [Space]
        [SerializeField] private CurrenciesDisplay GoldDiplay;

        private void Awake()
        {
            Manager.E_OnEnemySpawn.AddListener(OnEnemySpawned);

            Manager.E_OnPreBossReady.AddListener(OnBossSpawned);
        }

        /* Callbacks */

        private void OnEnemySpawned(GameObject enemy)
        {
            var health = enemy.GetComponent<HealthController>();

            health.E_OnZeroHealth.AddListener(() => OnEnemyDefeated(enemy));
        }

        private void OnBossSpawned(StageBossEventPayload payload)
        {
            var health = payload.GameObject.GetComponent<HealthController>();

            health.E_OnZeroHealth.AddListener(() => OnBossDefeated(payload));
        }

        private void OnBossDefeated(StageBossEventPayload payload)
        {
            var avatar = payload.GameObject.GetComponentInChildren<UnitAvatar>();

            BigDouble gold = App.Values.GoldPerStageBossAtStage(payload.StageSpawned);

            App.Inventory.Gold += gold;

            GoldDiplay.DisplayGoldTrail(avatar.Bounds.RandomPosition());
        }

        private void OnEnemyDefeated(GameObject enemy)
        {
            var avatar = enemy.GetComponentInChildren<UnitAvatar>();

            BigDouble gold = App.Values.GoldPerEnemyAtStage(App.GameState.Stage);

            App.Inventory.Gold += gold;

            GoldDiplay.DisplayGoldTrail(avatar.Bounds.RandomPosition());
        }
    }
}
