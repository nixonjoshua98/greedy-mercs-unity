using System.Numerics;

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
    [Space]
    [SerializeField] Text ShortDescriptionText;
    [SerializeField] Text LongDescriptionText;

    [Header("Prefabs")]
    [SerializeField] GameObject ErrorMessage;
    [SerializeField] GameObject BlankPanel;

    GameObject spawnedBlankPanel;

    int BuyAmount
    {
        get
        {
            if (RelicsTab.BuyAmount == -1)
                return Mathf.Max(1, Formulas.AffordRelicLevels(relicId));

            return RelicsTab.BuyAmount;
        }
    }

    void UpdateRow()
    {
        RelicStaticData data    = StaticData.GetRelic(relicId);
        UpgradeState relic      = GameState.Relics.GetRelic(relicId);

        ShortDescriptionText.text = "{effect} {type}"
            .Replace("{type}",      "<color=orange>" + Utils.Generic.BonusToString(data.bonusType) + "</color>")
            .Replace("{effect}",    "<color=orange>" + Utils.Format.FormatNumber(Formulas.CalcRelicEffect(relicId) * 100) + "%</color>");

        CostText.text               = Utils.Format.FormatNumber(Formulas.CalcRelicLevelUpCost(relicId, BuyAmount));
        LevelText.text              = "Level " + relic.level.ToString();
        LongDescriptionText.text    = data.description;
        BuyAmountText.text          = "x" + BuyAmount;
        NameText.text               = data.name;
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
        int levelsBuying = BuyAmount;

        BigInteger cost = Formulas.CalcRelicLevelUpCost(relicId, levelsBuying);

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
