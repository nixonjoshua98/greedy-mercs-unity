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
        [SerializeField] Text mercDpsText;
        [SerializeField] Text DiamondsText;

        void Awake()
        {
            Events.OnStageUpdate.AddListener(OnStageUpdate);
            Events.OnBossSpawned.AddListener(OnBossSpawned);

            UpdateStageText();

            InvokeRepeating("RegularUpdate", 0.0f, 0.25f);
        }

        void FixedUpdate()
        {
            GoldText.text       = Utils.Format.FormatNumber(GameState.Player.gold);
            DiamondsText.text   = GameState.Player.gems.ToString();
        }

        void RegularUpdate()
        {
            mercDpsText.text = Utils.Format.FormatNumber(StatsCache.TotalCharacterDPS);
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