
using System.Numerics;

using UnityEngine;

namespace GreedyMercs.BountyShop.UI
{
    using GreedyMercs.BountyShop.Data;
    using SimpleJSON;

    public class BSI_PrestigePoints : BountyShopItem
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

            BigInteger points = StatsCache.GetPrestigePoints(Mathf.CeilToInt(GameState.Player.maxPrestigeStage * data.GetFloat("maxStagePercent")));

            return string.Format("{0} Combat Experience", Utils.Format.FormatNumber(points < 100 ? 100 : points));
        }
    }
}
