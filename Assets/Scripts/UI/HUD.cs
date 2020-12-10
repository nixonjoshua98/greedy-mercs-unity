using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] Text StageText;
    [SerializeField] Text EnemyText;
    [SerializeField] Text GoldText;
    [SerializeField] Text DiamondsText;

    void Awake()
    {
        EventManager.OnStageUpdate.AddListener(OnStageUpdate);
    }

    void FixedUpdate()
    {
        GoldText.text = PlayerData.Gold.ToString();

        DiamondsText.text = "None";
    }

    void OnStageUpdate(int stage, int enemy)
    {
        StageText.text = stage.ToString();
        EnemyText.text = enemy.ToString();
    }
}
