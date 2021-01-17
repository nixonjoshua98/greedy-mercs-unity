using System.Collections.Generic;

using SimpleJSON;

namespace LootData
{
    public class LootState
    {
        Dictionary<LootID, UpgradeState> items;

        public int Count { get { return items.Count; } }

        public LootState(JSONNode node)
        {
            Update(node);
        }

        public void Update(JSONNode node)
        {
            if (node.HasKey("loot"))
            {
                items = new Dictionary<LootID, UpgradeState>();

                foreach (string itemId in node["loot"].Keys)
                {
                    items[(LootID)int.Parse(itemId)] = new UpgradeState { level = node["loot"][itemId].AsInt };
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
                LootItemSO data = StaticData.LootList.Get(relic.Key);

                switch (data.valueType)
                {
                    case ValueType.MULTIPLY:
                        bonuses[data.bonusType] = bonuses.GetOrVal(data.bonusType, 1) * Formulas.CalcLootItemEffect(relic.Key);
                        break;

                    case ValueType.ADDITIVE_FLAT_VAL:
                        bonuses[data.bonusType] = bonuses.GetOrVal(data.bonusType, 0) + Formulas.CalcLootItemEffect(relic.Key);
                        break;

                    case ValueType.ADDITIVE_PERCENT:
                        bonuses[data.bonusType] = bonuses.GetOrVal(data.bonusType, 0) + Formulas.CalcLootItemEffect(relic.Key);
                        break;
                }
            }

            return bonuses;
        }

        // === Helper Methods ===

        public UpgradeState Get(LootID loot)
        {
            return items[loot];
        }

        public void Add(LootID loot)
        {
            items[loot] = new UpgradeState { level = 1 };
        }

        public Dictionary<LootID, UpgradeState> Unlocked()
        {
            return items;
        }
    }
}