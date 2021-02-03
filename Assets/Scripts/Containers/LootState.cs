using System.Collections.Generic;

using UnityEngine;
using SimpleJSON;

namespace GreedyMercs
{
    public class LootState
    {
        const string FILE_NAME = "localloot01";

        Dictionary<LootID, UpgradeState> items;

        public int Count { get { return items.Count; } }

        public LootState()
        {
            items = new Dictionary<LootID, UpgradeState>();
        }

        public void UpdateWithServerData(JSONNode node)
        {
            Update(node);
        }

        public void UpdateWithLocalData()
        {
            if (!Utils.File.ReadJson(FILE_NAME, out JSONNode node))
                node = new JSONObject();

            Update(node);
        }

        public void Save()
        {
            Utils.File.WriteJson(FILE_NAME, ToJson());
        }

        public void Update(JSONNode node)
        {
            items = new Dictionary<LootID, UpgradeState>();

            foreach (string itemId in node.Keys)
            {
                items[(LootID)int.Parse(itemId)] = new UpgradeState { level = node[itemId].AsInt };
            }

            Save();
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

        public bool Contains(LootID loot) => items.ContainsKey(loot);

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