
using System.Numerics;

using UnityEngine;

namespace GreedyMercs.BountyShop.UI
{
    using GreedyMercs.BountyShop.Data;
    using SimpleJSON;

    public class BSP_PrestigePoints : BountyShopItem
    {
        void Awake()
        {
            item = BountyShopItemID.PRESTIGE_POINTS;
        }

        protected override void ProcessBoughtItem(JSONNode node)
        {
            GameState.Player.prestigePoints += BigInteger.Parse(node["receivedPrestigePoints"].Value);
        }

        protected override string GetDescription()
        {
            BountyShopItemSO data = StaticData.BountyShop.GetItem(item);

            int points = Mathf.CeilToInt(Mathf.Max(100, GameState.Player.maxPrestigeStage) * data.GetFloat("maxStagePercent"));

            return string.Format("{0} Combat Experience", Utils.Format.FormatNumber(StatsCache.GetPrestigePoints(points)));
        }
    }
}
