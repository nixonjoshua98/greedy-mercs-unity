using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using SimpleJSON;

public class RelicContainer
{
    Dictionary<RelicID, UpgradeState> relics;

    public RelicContainer(JSONNode node)
    {
        Update(node);
    }

    public void Update(JSONNode node)
    {
        relics = new Dictionary<RelicID, UpgradeState>();

        foreach (JSONNode chara in node["relics"].AsArray)
        {
            relics[(RelicID)int.Parse(chara["relicId"])] = JsonUtility.FromJson<UpgradeState>(chara.ToString());
        }
    }

    public JSONNode ToJson()
    {
        return Utils.Json.CreateJSONArray("relicId", relics);
    }

    public Dictionary<BonusType, double> CalculateBonuses()
    {
        Dictionary<BonusType, double> bonuses = new Dictionary<BonusType, double>();

        foreach (var relic in relics)
        {
            RelicStaticData staticData = StaticData.GetRelic(relic.Key);

            switch (staticData.bonusType)
            {
                case BonusType.CRIT_CHANCE:
                    bonuses[staticData.bonusType] = bonuses.GetValueOrDefault(staticData.bonusType, 0) + Formulas.CalcRelicEffect(relic.Key);
                    break;

                default:
                    bonuses[staticData.bonusType] = bonuses.GetValueOrDefault(staticData.bonusType, 1) * Formulas.CalcRelicEffect(relic.Key);
                    break;
            }
        }

        return bonuses;
    }

    // === 

    public int Count { get { return relics.Count; } }

    // === Helper Methods ===

    public UpgradeState GetRelic(RelicID relic)
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
}
