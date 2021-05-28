using System.Collections.Generic;

using UnityEngine;
using SimpleJSON;

namespace GreedyMercs
{
    using GM.Artefacts;

    public class LootState
    {
        Dictionary<int, UpgradeState> items;

        public int Count { get { return items.Count; } }

        public LootState()
        {
            items = new Dictionary<int, UpgradeState>();
        }

        public void UpdateWithServerData(JSONNode node)
        {
            Update(node);
        }

        public void Update(JSONNode node)
        {
            items = new Dictionary<int, UpgradeState>();

            foreach (string itemId in node.Keys)
            {
                items[int.Parse(itemId)] = new UpgradeState { level = node[itemId].AsInt };
            }
        }

        public Dictionary<BonusType, double> CalcBonuses()
        {
            Dictionary<BonusType, double> bonuses = new Dictionary<BonusType, double>();

            foreach (var relic in items)
            {
                 ArtefactData data = StaticData.Artefacts.Get((int)relic.Key);

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

        public UpgradeState Get(int loot)
        {
            return items[loot];
        }

        public bool Contains(int loot) => items.ContainsKey(loot);

        public void Add(int loot)
        {
            items[loot] = new UpgradeState { level = 1 };
        }

        public Dictionary<int, UpgradeState> Unlocked()
        {
            return items;
        }
    }
}