using System.Numerics;

using UnityEngine;
using UnityEngine.UI;

using SimpleJSON;

using LootData;

using Vector2 = UnityEngine.Vector2;

public class RelicRow : MonoBehaviour
{
    LootID relic;

    [Header("Components")]
    [SerializeField] Image icon;
    [Space]
    [SerializeField] Button buyButton;
    [Space]
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
            LootItemSO data     = StaticData.PrestigeItems.Get(relic);
            UpgradeState state      = GameState.PrestigeItems.Get(relic);

            return Mathf.Min(RelicsTab.BuyAmount, data.maxLevel - state.level);
        }
    }

    public void Init(LootItemSO data)
    {
        descText.text   = data.description;
        nameText.text   = data.name;

        relic = data.ItemID;

        Utils.UI.SetImageScaleW(icon, data.icon, 150.0f);

        UpdateRow();

        InvokeRepeating("UpdateRow", 0.3f, 0.3f);
    }

    void UpdateRow()
    {
        LootItemSO scriptable   = StaticData.PrestigeItems.Get(relic);
        UpgradeState state          = GameState.PrestigeItems.Get(relic);

        UpdateEffectText();

        levelText.text  = "Level " + state.level.ToString();

        if (state.level < scriptable.maxLevel)
        {
            string cost = Utils.Format.FormatNumber(Formulas.CalcPrestigeItemLevelUpCost(relic, BuyAmount));

            costText.text = string.Format("x{0}\n{1}", BuyAmount, cost);
        }

        else
            costText.text = "MAX";


        buyButton.interactable = state.level < scriptable.maxLevel;
    }

    void UpdateEffectText()
    {
        LootItemSO scriptable = StaticData.PrestigeItems.Get(relic);

        double effect = Formulas.CalcPrestigeItemEffect(relic);

        switch (scriptable.valueType)
        {
            case ValueType.MULTIPLY:
                effectText.text = Utils.Format.FormatNumber(effect * 100) + "%";
                break;

            case ValueType.ADDITIVE_PERCENT:
                effectText.text = "+ " + Utils.Format.FormatNumber(effect * 100) + "%";
                break;

            case ValueType.ADDITIVE_FLAT_VAL:
                effectText.text = "+ " + Utils.Format.FormatNumber(effect);
                break;
        }

        effectText.text += " " + Utils.Generic.BonusToString(scriptable.bonusType);
    }

    // === Button Callbacks

    public void OnBuy()
    {
        int levelsBuying = BuyAmount;

        LootItemSO data = StaticData.PrestigeItems.Get(relic);
        UpgradeState state  = GameState.PrestigeItems.Get(relic);

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
            JSONNode node       = Utils.Json.Decompress(compressed);
            UpgradeState state  = GameState.PrestigeItems.Get(relic);

            state.level = node["itemLevel"].AsInt;

            GameState.Player.prestigePoints = BigInteger.Parse(node["prestigePoints"].Value);
        }

        else
        {
            Utils.UI.ShowMessage("Server Error ", "Failed to upgrade item :(");
        }

        UpdateRow();

        Destroy(spawnedBlankPanel);
    }
}
