using System.Numerics;

using UnityEngine;
using UnityEngine.UI;

using SimpleJSON;

using PrestigeItemsData;

public class RelicRow : MonoBehaviour
{
    PrestigeItemID relic;

    [Header("Components")]
    [SerializeField] Image icon;
    [Space]
    [SerializeField] Button buyButton;
    [Space]
    [SerializeField] Text buyText;
    [SerializeField] Text costText;
    [SerializeField] Text levelText;
    [SerializeField] Text nameText;
    [SerializeField] Text effectText;
    [SerializeField] Text descText;

    [Header("Prefabs")]
    [SerializeField] GameObject BlankPanel;

    GameObject spawnedBlankPanel;

    int BuyAmount {
        get 
        {
            PrestigeItemSO data        = StaticData.PrestigeItems.Get(relic);
            UpgradeState state  = GameState.PrestigeItems.Get(relic);

            return Mathf.Min(RelicsTab.BuyAmount, data.maxLevel - state.level);
        }
    }

    public void Init(PrestigeItemSO data)
    {
        descText.text   = data.description;
        nameText.text   = data.name;
        icon.sprite     = data.icon;

        relic = data.ItemID;

        UpdateRow();

        InvokeRepeating("UpdateRow", 0.3f, 0.3f);
    }

    void UpdateRow()
    {
        PrestigeItemSO scriptable  = StaticData.PrestigeItems.Get(relic);
        UpgradeState state  = GameState.PrestigeItems.Get(relic);

        effectText.text = Utils.Format.FormatNumber(Formulas.CalcPrestigeItemEffect(relic) * 100) + "% " + Utils.Generic.BonusToString(scriptable.bonusType);
        costText.text   = state.level >= scriptable.maxLevel ? "MAX" : Utils.Format.FormatNumber(Formulas.CalcPrestigeItemLevelUpCost(relic, BuyAmount));
        levelText.text  = "Level " + state.level.ToString();
        buyText.text    = state.level >= scriptable.maxLevel ? "" : "x" + BuyAmount.ToString();

        buyButton.interactable = state.level < scriptable.maxLevel;
    }

    // === Button Callbacks

    public void OnBuy()
    {
        int levelsBuying = BuyAmount;

        PrestigeItemSO data            = StaticData.PrestigeItems.Get(relic);
        UpgradeState state      = GameState.PrestigeItems.Get(relic);

        BigInteger cost = Formulas.CalcPrestigeItemLevelUpCost(relic, levelsBuying);

        if (levelsBuying > 0 && GameState.Player.prestigePoints >= cost && (state.level + levelsBuying) <= data.maxLevel)
        {
            spawnedBlankPanel = Utils.UI.Instantiate(BlankPanel, UnityEngine.Vector3.zero);

            JSONNode node = Utils.Json.GetDeviceNode();

            node.Add("itemId", (int)relic);
            node.Add("buyLevels", BuyAmount);

            Server.UpgradePrestigeItem(this, OnRelicUpgradeCallback, node);
        }
    }

    void OnRelicUpgradeCallback(long code, string compressed)
    {
        if (code == 200)
        {
            JSONNode node = Utils.Json.Decompress(compressed);
            UpgradeState state = GameState.PrestigeItems.Get(relic);

            state.level = node["itemLevel"].AsInt;

            GameState.Player.prestigePoints = BigInteger.Parse(node["prestigePoints"].Value);
        }

        else if (code == 400 || code == 500)
        {
            JSONNode node = Utils.Json.Decompress(compressed);

            Utils.UI.ShowMessage("Item Upgrade Error", node["message"].Value.ToString());
        }

        else
        {
            Utils.UI.ShowMessage("Server Error ", "Failed to upgrade item :(");
        }

        UpdateRow();

        Destroy(spawnedBlankPanel);
    }
}
