using TMPro;
using UnityEngine;

namespace SRC.UI.HUD
{
    public class KillCountHUD : SRC.Core.GMMonoBehaviour
    {
        [SerializeField] GameManager Manager;

        [Header("Components")]
        [SerializeField] TMP_Text KillCountText;

        void Awake()
        {
            UpdateText();

            Manager.E_OnEnemySpawn.AddListener(OnEnemySpawned);
        }

        void UpdateText()
        {
            KillCountText.text = $"{App.GameState.EnemiesDefeated + 1} / {App.GameState.EnemiesPerStage}";
        }

        /* Callbacks */

        void OnEnemySpawned(GameObject _)
        {
            UpdateText();
        }
    }
}
