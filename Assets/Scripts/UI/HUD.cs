using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GreedyMercs
{
    public class HUD : MonoBehaviour
    {
        [SerializeField] Text StageText;
        [SerializeField] Text GoldText;

        void Awake()
        {
            Events.OnStageUpdate.AddListener(OnStageUpdate);
            Events.OnBossSpawned.AddListener(OnBossSpawned);

            UpdateStageText();
        }

        void FixedUpdate()
        {
            GoldText.text = Utils.Format.FormatNumber(GameState.Player.gold);
        }

        void UpdateStageText()
        {
            StageText.text = GameState.Stage.stage.ToString() + " | " + GameState.Stage.enemy.ToString();
        }

        void OnStageUpdate()
        {
            UpdateStageText();
        }

        void OnBossSpawned(GameObject _)
        {
            StageText.text = "BOSS";
        }
    }
}