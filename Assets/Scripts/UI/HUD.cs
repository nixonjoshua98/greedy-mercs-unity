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
        [SerializeField] Text tapDamageText;

        void Awake()
        {
            Events.OnStageUpdate.AddListener(OnStageUpdate);
            Events.OnBossSpawned.AddListener(OnBossSpawned);

            UpdateStageText();

            InvokeRepeating("UpdateUI", 0.0f, 0.25f);
        }

        void FixedUpdate()
        {
            GoldText.text = Utils.Format.FormatNumber(GameState.Player.gold);
        }

        void UpdateUI()
        {
            mercDpsText.text    = Utils.Format.FormatNumber(StatsCache.TotalCharacterDPS);
            tapDamageText.text  = Utils.Format.FormatNumber(StatsCache.GetTapDamage());
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