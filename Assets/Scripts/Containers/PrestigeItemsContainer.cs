using System.Collections.Generic;

using UnityEngine;

using SimpleJSON;

using LootData;

public class PrestigeItemsContainer
{
    Dictionary<LootID, UpgradeState> items;

    public int Count { get { return items.Count; } }

    public PrestigeItemsContainer(JSONNode node)
    {
        Update(node);
    }

    public void Update(JSONNode node)
    {
        if (node.HasKey("prestigeItems"))
        {
            items = new Dictionary<LootID, UpgradeState>();

            foreach (string itemId in node["prestigeItems"].Keys)
            {
                items[(LootID)int.Parse(itemId)] = new UpgradeState { level = node["prestigeItems"][itemId].AsInt };
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
            LootItemSO data = StaticData.PrestigeItems.Get(relic.Key);

            switch (data.valueType)
            {
                case ValueType.MULTIPLY:
                    bonuses[data.bonusType] = bonuses.GetOrVal(data.bonusType, 1) * Formulas.CalcPrestigeItemEffect(relic.Key);
                    break;

                case ValueType.ADDITIVE_FLAT_VAL:
                    bonuses[data.bonusType] = bonuses.GetOrVal(data.bonusType, 0) + Formulas.CalcPrestigeItemEffect(relic.Key);
                    break;

                case ValueType.ADDITIVE_PERCENT:
                    bonuses[data.bonusType] = bonuses.GetOrVal(data.bonusType, 0) + Formulas.CalcPrestigeItemEffect(relic.Key);
                    break;
            }
        }

        return bonuses;
    }

    // === Helper Methods ===

    public UpgradeState Get(LootID relic)
    {
        return items[relic];
    }

    public void Add(LootID relic)
    {
        items[relic] = new UpgradeState { level = 1 };
    }

    public Dictionary<LootID, UpgradeState> Unlocked()
    {
        return items;
    }
}
