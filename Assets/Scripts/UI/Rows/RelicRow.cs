using System.Numerics;

using UnityEngine;
using UnityEngine.UI;

using SimpleJSON;

using RelicID           = RelicData.RelicID;
using RelicStaticData   = RelicData.RelicStaticData;

public class RelicRow : UpgradeRow
{
    [Space]

    [SerializeField] RelicID relicId;

    [Header("Components")]
    [SerializeField] Text NameText;
    [SerializeField] Text ShortDescriptionText;
    [SerializeField] Text LongDescriptionText;

    [Header("Prefabs")]
    [SerializeField] GameObject ErrorMessage;
    [SerializeField] GameObject BlankPanel;

    GameObject spawnedBlankPanel;

    public override bool IsUnlocked { get { return GameState.Relics.TryGetRelic(relicId, out var _); } }

    void Start()
    {
        RelicStaticData staticData = StaticData.Relics.Get(relicId);

        LongDescriptionText.text    = staticData.description;
        NameText.text               = staticData.name;
    }

    protected override int GetBuyAmount()
    {
        if (RelicsTab.BuyAmount == -1)
            return Formulas.AffordRelicLevels(relicId);

        return Mathf.Min(RelicsTab.BuyAmount, Formulas.AffordRelicLevels(relicId));
    }

    public override void UpdateRow()
    {
        RelicStaticData data = StaticData.Relics.Get(relicId);
        UpgradeState state   = GameState.Relics.GetRelic(relicId);

        ShortDescriptionText.text = "{effect} {type}"
            .Replace("{type}", "<color=orange>" + Utils.Generic.BonusToString(data.bonusType) + "</color>")
            .Replace("{effect}", "<color=orange>" + Utils.Format.FormatNumber(Formulas.CalcRelicEffect(relicId) * 100) + "%</color>");

        CostText.text = state.level >= data.maxLevel ? "MAX" : Utils.Format.FormatNumber(Formulas.CalcRelicLevelUpCost(relicId, BuyAmount));

        UpdateText(state, data.maxLevel);
    }

    // === Button Callbacks

    public override void OnBuy()
    {
        int levelsBuying = BuyAmount;

        RelicStaticData data    = StaticData.Relics.Get(relicId);
        UpgradeState state      = GameState.Relics.GetRelic(relicId);

        BigInteger cost = Formulas.CalcRelicLevelUpCost(relicId, levelsBuying);

        if (levelsBuying > 0 && GameState.Player.prestigePoints >= cost && state.level < data.maxLevel)
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
            JSONNode node = Utils.Json.Decode(compressed);

            UpgradeState state = GameState.Relics.GetRelic(relicId);

            state.level = node["relicLevel"].AsInt;

            GameState.Player.prestigePoints = BigInteger.Parse(node["prestigePoints"].Value);
        }

        else
        {
            Utils.UI.ShowError(ErrorMessage, "Server Error " + code, "Failed to upgrade relic :(");
        }

        UpdateRow();

        Destroy(spawnedBlankPanel);
    }
}
