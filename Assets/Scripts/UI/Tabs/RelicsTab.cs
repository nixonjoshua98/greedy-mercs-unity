using System.Numerics;

using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;

using SimpleJSON;

using LootData;

using Vector3 = UnityEngine.Vector3;

public class RelicsTab : MonoBehaviour
{
    static RelicsTab Instance = null;

    [Header("GameObjects")]
    [SerializeField] GameObject rowParent;

    [Space]
    [SerializeField] BuyAmountController buyAmount;

    [Header("Components")]
    [SerializeField] Button BuyRelicsButton;

    [Header("Text Components")]
    [SerializeField] Text PrestigePointText;
    [SerializeField] Text RelicCostText;

    [Header("Prefabs")]
    [SerializeField] GameObject RelicRowObject;

    [SerializeField] GameObject BlankPanel;

    public static int BuyAmount { get { return Instance.buyAmount.BuyAmount; } }

    GameObject spawnedBlankPanel;

    List<RelicRow> rows;

    void Awake()
    {
        Instance = this;

        rows = new List<RelicRow>();

        foreach (var relic in GameState.PrestigeItems.Unlocked())
        {
            AddRow(relic.Key);
        }
    }

    void AddRow(LootID item)
    {
        LootItemSO scriptable = StaticData.PrestigeItems.Get(item);

        GameObject inst = Utils.UI.Instantiate(RelicRowObject, rowParent.transform, Vector3.zero);

        RelicRow row = inst.GetComponent<RelicRow>();

        row.Init(scriptable);

        rows.Add(row);
    }

    void FixedUpdate()
    {
        PrestigePointText.text = Utils.Format.FormatNumber(GameState.Player.prestigePoints) + " (<color=orange>+" 
            + Utils.Format.FormatNumber(StatsCache.GetPrestigePoints(GameState.Stage.stage)) + "</color>)";

        BuyRelicsButton.interactable = GameState.PrestigeItems.Count < StaticData.PrestigeItems.Count;

        if (GameState.PrestigeItems.Count < StaticData.PrestigeItems.Count)
        {
            RelicCostText.text = string.Format("Buy Relic\n{0}", Utils.Format.FormatNumber(Formulas.CalcNextPrestigeItemCost(GameState.PrestigeItems.Count)));
        }
        else
            RelicCostText.text = "All Relics Obtained";
    }

    // === Button Callbacks ===

    public void OnBuyRelic()
    {
        if (GameState.Player.prestigePoints < Formulas.CalcNextPrestigeItemCost(GameState.PrestigeItems.Count))
        {
            Utils.UI.ShowMessage("Poor Player Alert", "You cannot afford to buy a new item");
        }
        else if (GameState.PrestigeItems.Count < StaticData.PrestigeItems.Count)
        {
            spawnedBlankPanel = Utils.UI.Instantiate(BlankPanel, Vector3.zero);

            Server.BuyPrestigeItem(this, OnBuyCallback, Utils.Json.GetDeviceNode());
        }
    }

    public void OnBuyCallback(long code, string data)
    {
        if (code == 200)
        {
            JSONNode node = Utils.Json.Decompress(data);

            LootID item = (LootID)node["itemBought"].AsInt;

            GameState.Player.Update(node);

            GameState.PrestigeItems.Add(item);

            AddRow(item);
        }

        else
        {
            Utils.UI.ShowMessage("Buy Item", "A connection to the server is required to buy a new item");
        }

        Destroy(spawnedBlankPanel);
    }
}
