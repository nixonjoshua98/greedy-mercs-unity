
using System.Numerics;

using UnityEngine;

namespace GreedyMercs.BountyShop.UI
{
    using GreedyMercs.BountyShop.Data;
    using SimpleJSON;

    public class BSI_PrestigePoints : BountyShopItem
    {
        protected override void Awake()
        {
            item = BountyShopItemID.PRESTIGE_POINTS;
        }

        protected override void ProcessBoughtItem(JSONNode node)
        {
            GameState.Player.prestigePoints += BigInteger.Parse(node["prestigePointsReceived"].Value);
        }

        protected override string GetDescription()
        {
            BountyShopItemSO data = StaticData.BountyShop.GetItem(item);

            BigInteger points = StatsCache.GetPrestigePoints(Mathf.CeilToInt(GameState.LifetimeStats.maxPrestigeStage * data.GetFloat("maxStagePercent")));

            return string.Format("{0} Runestones", Utils.Format.FormatNumber(points < 100 ? 100 : points));
        }
    }
}
