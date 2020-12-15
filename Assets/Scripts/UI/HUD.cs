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
    }

    void FixedUpdate()
    {
        GoldText.text = UtilsClass.DoubleToString(PlayerData.gold);

        DiamondsText.text = "None";
    }

    void OnStageUpdate(int stage, int enemy)
    {
        StageText.text = stage.ToString() + " | " + enemy.ToString();
    }

    void OnBossSpawned(GameObject _)
    {
        StageText.text = "BOSS";
    }
}
