using System.Numerics;

using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;

using SimpleJSON;

using PrestigeItemsData;

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
    [SerializeField] Text PrestigeButtonText;
    [SerializeField] Text RelicCostText;

    [Header("Prefabs")]
    [SerializeField] GameObject RelicRowObject;
    [SerializeField] GameObject PrestigePanelObject;

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

    void AddRow(PrestigeItemID item)
    {
        PrestigeItemSO scriptable = StaticData.PrestigeItems.Get(item);

        GameObject inst = Utils.UI.Instantiate(RelicRowObject, rowParent.transform, Vector3.zero);

        RelicRow row = inst.GetComponent<RelicRow>();

        row.Init(scriptable);

        rows.Add(row);
    }

    void FixedUpdate()
    {
        PrestigeButtonText.text = GameState.Stage.stage >= StageState.MIN_PRESTIGE_STAGE ? "Cash Out" : "Locked Stage " + StageState.MIN_PRESTIGE_STAGE.ToString();

        PrestigePointText.text = Utils.Format.FormatNumber(GameState.Player.prestigePoints) + " (<color=orange>+" 
            + Utils.Format.FormatNumber(StatsCache.GetPrestigePoints(GameState.Stage.stage)) + "</color>)";

        RelicCostText.text = GameState.PrestigeItems.Count < StaticData.PrestigeItems.Count ? Utils.Format.FormatNumber(Formulas.CalcNextPrestigeItemCost(GameState.PrestigeItems.Count)) : "MAX";

        BuyRelicsButton.interactable = GameState.PrestigeItems.Count < StaticData.PrestigeItems.Count;
    }

    // === Button Callbacks ===

    public void ShowPrestigePanel()
    {
        if (GameState.Stage.stage >= StageState.MIN_PRESTIGE_STAGE)
            Utils.UI.Instantiate(PrestigePanelObject, Vector3.zero);
    }

    public void OnBuyRelic()
    {
        if (GameState.Player.prestigePoints < Formulas.CalcNextPrestigeItemCost(GameState.PrestigeItems.Count))
        {
            Utils.UI.ShowMessage("Poor Player Alert", "You cannot afford to buy a new item");
        }
        else
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

            PrestigeItemID item = (PrestigeItemID)node["itemBought"].AsInt;

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
