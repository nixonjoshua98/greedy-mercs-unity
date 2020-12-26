
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

using SimpleJSON;

public class RelicsTab : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] GameObject rowParent;

    [Header("Components")]
    [SerializeField] Button BuyRelicsButton;

    [Header("Text Components")]
    [SerializeField] Text PrestigePointText;
    [SerializeField] Text PrestigeButtonText;
    [SerializeField] Text RelicCostText;

    [Header("Prefabs")]
    [SerializeField] GameObject BlankPanel;
    [SerializeField] GameObject ErrorMessage;

    GameObject spawnedBlankPanel;

    List<RelicRow> rows;

    void Awake()
    {
        rows = new List<RelicRow>();

        for (int i = 0; i < rowParent.transform.childCount; ++i)
        {
            Transform child = rowParent.transform.GetChild(i);

            if (child.TryGetComponent(out RelicRow row))
                rows.Add(row);
        }
    }

    void OnEnable()
    {
        InvokeRepeating("OnUpdate", 0.0f, 0.5f);
    }

    void OnDisable()
    {
        if (IsInvoking("OnUpdate"))
            CancelInvoke("OnUpdate");
    }

    void OnUpdate()
    {
        PrestigeButtonText.text = GameState.Stage.stage >= StageData.MIN_PRESTIGE_STAGE ? "Cash Out" : "Locked Stage " + StageData.MIN_PRESTIGE_STAGE.ToString();

        PrestigePointText.text = Utils.Format.FormatNumber(GameState.Player.prestigePoints) + " (<color=orange>+" 
            + Utils.Format.FormatNumber(Formulas.CalcPrestigePoints(GameState.Stage.stage)) + "</color>)";

        RelicCostText.text = GameState.Relics.Count < StaticData.numRelics ? Utils.Format.FormatNumber(Formulas.CalcNextRelicCost(GameState.Relics.Count)) : "MAX";

        BuyRelicsButton.interactable = GameState.Relics.Count < StaticData.numRelics;

        UpdateRows();
    }

    void UpdateRows()
    {
        foreach (RelicRow row in rows)
            row.gameObject.SetActive(row.TryUpdate());
    }

    // === Button Callbacks ===

    public void OnPrestige()
    {
        if (GameState.Stage.stage < StageData.MIN_PRESTIGE_STAGE)
            return;

        PrestigeManager.StartPrestige();
    }

    public void OnBuyRelic()
    {
        spawnedBlankPanel = Utils.UI.Instantiate(BlankPanel, Vector3.zero);

        Server.BuyRelic(this, OnBuyRelicCallback, Utils.Json.GetDeviceNode());
    }

    public void OnBuyRelicCallback(long code, string data)
    {
        if (code == 200)
        {
            JSONNode node = Utils.Json.Decompress(data);

            GameState.Player.Update(node);

            GameState.Relics.AddRelic((RelicID)node["relicBought"].AsInt);
        }

        else
        {
            Utils.UI.ShowError(ErrorMessage, "Relic", "A connection to the server is required");
        }

        Destroy(spawnedBlankPanel);
    }
}
