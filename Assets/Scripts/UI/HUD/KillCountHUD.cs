using TMPro;
using UnityEngine;

namespace SRC.UI.HUD
{
    public class KillCountHUD : SRC.Core.GMMonoBehaviour
    {
        [SerializeField] private GameManager Manager;

        [Header("Components")]
        [SerializeField] private TMP_Text KillCountText;

        private void Awake()
        {
            UpdateText();

            Manager.E_OnEnemySpawn.AddListener(OnEnemySpawned);
        }

        private void UpdateText()
        {
            KillCountText.text = $"{App.GameState.EnemiesDefeated + 1} / {App.GameState.EnemiesPerStage}";
        }

        /* Callbacks */

        private void OnEnemySpawned(GameObject _)
        {
            UpdateText();
        }
    }
}
