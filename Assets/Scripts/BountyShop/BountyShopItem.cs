using System.Numerics;

using SimpleJSON;

using UnityEngine;
using UnityEngine.UI;

namespace GreedyMercs.BountyShop.UI
{
    using GreedyMercs.BountyShop.Data;

    public class BountyShopItem : BountyShopItemBase
    {
        [Header("Components")]
        [SerializeField] Text descriptionText;

        protected override void UpdateUI()
        {
            base.UpdateUI();

            descriptionText.text = GetDescription();
        }

        protected void ProcessBoughtItem(JSONNode node)
        {
            switch (item)
            {
                case BountyShopItemID.PRESTIGE_POINTS:
                    GameState.Inventory.prestigePoints += BigInteger.Parse(node["prestigePointsReceived"].Value);
                    break;

                case BountyShopItemID.GEMS:
                    GameState.Inventory.gems += node["gemsReceived"].AsLong;
                    break;

                case BountyShopItemID.ARMOURY_POINTS:
                    GameState.Inventory.armouryPoints += node["armouryPointsReceived"].AsLong;
                    break;
            }
        }

        string GetDescription()
        {
            switch (item)
            {
                case BountyShopItemID.PRESTIGE_POINTS:
                    BountyShopItemData data = StaticData.BountyShop.Get(item);
                    BigInteger points       = StatsCache.GetPrestigePoints(Mathf.CeilToInt(GameState.LifetimeStats.maxPrestigeStage * data.GetFloat("maxStagePercent")));

                    return string.Format("{0} Runestones", Utils.Format.FormatNumber(points < 100 ? 100 : points));

                case BountyShopItemID.GEMS:
                    return string.Format("{0} Gems", StaticData.BountyShop.Get(item).GetInt("gems"));

                case BountyShopItemID.ARMOURY_POINTS:
                    return string.Format("{0} Armoury Points", StaticData.BountyShop.Get(item).GetInt("armouryPoints"));
            }

            return "Default Description";
        }


        public void OnClick()
        {
            bool inStock = itemData.dailyPurchaseLimit > itemState.dailyPurchased;

            if (GameState.BountyShop.IsValid && inStock)
            {
                JSONNode node = Utils.Json.GetDeviceNode();

                node.Add("itemId", (int)item);

                Server.BuyBountyShopItem(ServerCallback, node);
            }
        }

        void ServerCallback(long code, string compressed)
        {
            if (code == 200)
            {
                GameState.BountyShop.ProcessPurchase(item);

                ProcessBoughtItem(Utils.Json.Decompress(compressed));
            }

            UpdateUI();
        }
    }
}
