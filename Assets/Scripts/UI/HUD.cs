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

            UpdateInterface();

            InvokeRepeating("RegularUpdate", 0.0f, 0.5f);
        }

        void FixedUpdate()
        {
            GoldText.text = Utils.Format.FormatNumber(GameState.Player.gold);

            DiamondsText.text = "None";
        }

        void RegularUpdate()
        {
            mercDpsText.text = Utils.Format.FormatNumber(StatsCache.TotalCharacterDPS);
        }

        void UpdateInterface()
        {
            StageText.text = GameState.Stage.stage.ToString() + " | " + GameState.Stage.enemy.ToString();
        }

        void OnStageUpdate()
        {
            UpdateInterface();
        }

        void OnBossSpawned(GameObject _)
        {
            StageText.text = "BOSS";
        }
    }
}