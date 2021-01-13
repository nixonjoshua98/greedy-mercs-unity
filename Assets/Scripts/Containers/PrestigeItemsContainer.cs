using System.Collections.Generic;

using UnityEngine;

using SimpleJSON;

using PrestigeItemsData;

public class PrestigeItemsContainer
{
    Dictionary<PrestigeItemID, UpgradeState> items;

    public int Count { get { return items.Count; } }

    public PrestigeItemsContainer(JSONNode node)
    {
        Update(node);
    }

    public void Update(JSONNode node)
    {
        if (node.HasKey("prestigeItems"))
        {
            items = new Dictionary<PrestigeItemID, UpgradeState>();

            foreach (string itemId in node["prestigeItems"].Keys)
            {
                items[(PrestigeItemID)int.Parse(itemId)] = new UpgradeState { level = node["prestigeItems"][itemId].AsInt };
            }
        }
    }

    public JSONNode ToJson()
    {
        JSONNode node = new JSONObject();

        foreach (var item in items)
        {
            node.Add(((int)item.Key).ToString(), item.Value.level);
        }

        return node;
    }

    public Dictionary<BonusType, double> CalcBonuses()
    {
        Dictionary<BonusType, double> bonuses = new Dictionary<BonusType, double>();

        foreach (var relic in items)
        {
            PrestigeItemSO data = StaticData.PrestigeItems.Get(relic.Key);

            switch (data.bonusType)
            {
                case BonusType.CRIT_CHANCE:
                    bonuses[data.bonusType] = bonuses.GetOrVal(data.bonusType, 0) + Formulas.CalcPrestigeItemEffect(relic.Key);
                    break;

                default:
                    bonuses[data.bonusType] = bonuses.GetOrVal(data.bonusType, 1) * Formulas.CalcPrestigeItemEffect(relic.Key);
                    break;
            }
        }

        return bonuses;
    }

    // === Helper Methods ===

    public UpgradeState Get(PrestigeItemID relic)
    {
        return items[relic];
    }

    public void Add(PrestigeItemID relic)
    {
        items[relic] = new UpgradeState { level = 1 };
    }

    public Dictionary<PrestigeItemID, UpgradeState> Unlocked()
    {
        return items;
    }
}
