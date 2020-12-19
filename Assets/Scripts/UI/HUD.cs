using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] Text StageText;
    [SerializeField] Text GoldText;
    [SerializeField] Text DiamondsText;

    void Awake()
    {
        EventManager.OnStageUpdate.AddListener(OnStageUpdate);
        EventManager.OnBossSpawned.AddListener(OnBossSpawned);

        UpdateInterface();
    }

    void FixedUpdate()
    {
        GoldText.text = Utils.Format.DoubleToString(GameState.player.gold);

        DiamondsText.text = "None";
    }

    void UpdateInterface()
    {
        StageText.text = GameState.stage.stage.ToString() + " | " + GameState.stage.enemy.ToString();
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
