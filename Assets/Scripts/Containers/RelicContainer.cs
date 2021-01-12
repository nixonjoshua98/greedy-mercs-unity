using System.Collections.Generic;

using UnityEngine;

using SimpleJSON;

using RelicData;

public class RelicContainer
{
    Dictionary<RelicID, UpgradeState> relics;

    public int Count { get { return relics.Count; } }

    public RelicContainer(JSONNode node)
    {
        Update(node);
    }

    public void Update(JSONNode node)
    {
        if (node.HasKey("relics"))
        {
            relics = new Dictionary<RelicID, UpgradeState>();

            foreach (string relicId in node["relics"].Keys)
            {
                relics[(RelicID)int.Parse(relicId)] = new UpgradeState { level = node["relics"][relicId].AsInt };
            }
        }
    }

    public JSONNode ToJson()
    {
        JSONNode node = new JSONObject();

        foreach (var relic in relics)
        {
            node.Add(((int)relic.Key).ToString(), relic.Value.level);
        }

        return node;
    }

    public Dictionary<BonusType, double> CalculateBonuses()
    {
        Dictionary<BonusType, double> bonuses = new Dictionary<BonusType, double>();

        foreach (var relic in relics)
        {
            RelicSO data = StaticData.Relics.Get(relic.Key);

            switch (data.bonusType)
            {
                case BonusType.CRIT_CHANCE:
                    bonuses[data.bonusType] = bonuses.GetValueOrDefault(data.bonusType, 0) + Formulas.CalcRelicEffect(relic.Key);
                    break;

                default:
                    bonuses[data.bonusType] = bonuses.GetValueOrDefault(data.bonusType, 1) * Formulas.CalcRelicEffect(relic.Key);
                    break;
            }
        }

        return bonuses;
    }

    // === Helper Methods ===

    public UpgradeState Get(RelicID relic)
    {
        return relics[relic];
    }

    public bool TryGetRelic(RelicID relic, out UpgradeState state)
    {
        return relics.TryGetValue(relic, out state);
    }

    public void AddRelic(RelicID relic)
    {
        relics[relic] = new UpgradeState { level = 1 };
    }

    public Dictionary<RelicID, UpgradeState> Unlocked()
    {
        return relics;
    }
}
