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
        [SerializeField] Text itemQuantityText;

        protected override void UpdateUI()
        {
            base.UpdateUI();

            SetQuantity();
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
            }
        }

        void SetQuantity()
        {
            itemQuantityText.text = "x";

            switch (item)
            {
                case BountyShopItemID.PRESTIGE_POINTS:
                    BountyShopItemData data = StaticData.BountyShop.Get(item);
                    BigInteger points = StatsCache.GetPrestigePoints(Mathf.CeilToInt(GameState.LifetimeStats.maxPrestigeStage * data.GetFloat("maxStagePercent")));

                    itemQuantityText.text += Utils.Format.FormatNumber(points < 100 ? 100 : points);
                    break;

                case BountyShopItemID.GEMS:
                    itemQuantityText.text += StaticData.BountyShop.Get(item).GetInt("gems").ToString();
                    break;
            }
        }


        public void OnClick()
        {
            bool inStock = itemData.dailyPurchaseLimit > itemState.dailyPurchased;

            if (GameState.BountyShop.IsValid && inStock)
            {
                JSONNode node = Utils.Json.GetDeviceInfo();

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
