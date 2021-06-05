using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GM
{
    public class HUD : MonoBehaviour
    {
        [SerializeField] Text StageText;
        [SerializeField] Text GoldText;

        void Start()
        {
            GlobalEvents.OnStageUpdate.AddListener(OnStageUpdate);

            // Temp. Derived events do not show in the inspector
            FindObjectOfType<StageBossController>().OnBossSpawn.AddListener(OnBossSpawn);

            UpdateStageText();
        }

        void FixedUpdate()
        {
            GoldText.text = Utils.Format.FormatNumber(GameState.Player.gold);
        }

        void UpdateStageText()
        {
            StageText.text = GameState.Stage.currentStage.ToString() + " | " + GameState.Stage.currentEnemy.ToString();
        }

        void OnStageUpdate()
        {
            UpdateStageText();
        }

        // Event
        public void OnBossSpawn(GameObject obj)
        {
            StageText.text = "BOSS";
        }
    }
}