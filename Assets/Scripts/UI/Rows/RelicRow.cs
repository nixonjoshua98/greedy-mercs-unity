﻿using System.Numerics;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using SimpleJSON;

public class RelicRow : MonoBehaviour
{
    [SerializeField] RelicID relicId;

    [Header("Components")]
    [SerializeField] Text NameText;
    [SerializeField] Text CostText;
    [SerializeField] Text LevelText;
    [SerializeField] Text BuyAmountText;
    [SerializeField] Text DescriptionText;

    [Header("Prefabs")]
    [SerializeField] GameObject ErrorMessage;
    [SerializeField] GameObject BlankPanel;

    GameObject spawnedBlankPanel;

    int BuyAmount
    {
        get
        {
            if (RelicsTab.BuyAmount == -1)
                return Mathf.Max(1, Formulas.CalcAffordableRelicLevels(relicId));

            return RelicsTab.BuyAmount;
        }
    }

    void UpdateRow()
    {
        RelicStaticData data = StaticData.GetRelic(relicId);

        UpgradeState relic = GameState.Relics.GetRelic(relicId);

        NameText.text = data.name;

        DescriptionText.text = data.description
            .Replace("{relicEffect}", "<color=orange>" + Formulas.CalcRelicEffect(relicId) + "x</color>");

        CostText.text = Utils.Format.FormatNumber(Formulas.CalcRelicLevelUpCost(relicId, BuyAmount));

        LevelText.text = "Level " + relic.level.ToString();

        BuyAmountText.text = "x" + BuyAmount;
    }

    public bool TryUpdate()
    {
        if (GameState.Relics.TryGetRelic(relicId, out var _))
        {
            UpdateRow();

            return true;
        }

        return false;
    }

    // === Button Callbacks

    public void OnRelicUpgrade()
    {
        BigInteger cost = Formulas.CalcRelicLevelUpCost(relicId, BuyAmount);

        if (GameState.Player.prestigePoints >= cost)
        {
            spawnedBlankPanel = Utils.UI.Instantiate(BlankPanel, UnityEngine.Vector3.zero);

            JSONNode node = Utils.Json.GetDeviceNode();

            node.Add("relicId", (int)relicId);
            node.Add("buyLevels", BuyAmount);

            Server.UpgradeRelic(this, OnRelicUpgradeCallback, node);
        }
    }

    void OnRelicUpgradeCallback(long code, string compressed)
    {
        if (code == 200)
        {
            JSONNode node = Utils.Json.Decompress(compressed);

            UpgradeState state = GameState.Relics.GetRelic(relicId);

            state.level = node["relicLevel"].AsInt;

            GameState.Player.prestigePoints = BigInteger.Parse(node["prestigePoints"]);
        }
        else
        {
            Utils.UI.ShowError(ErrorMessage, "Relic Upgrade", "Failed to upgrade relic");
        }

        UpdateRow();

        Destroy(spawnedBlankPanel);
    }
}
