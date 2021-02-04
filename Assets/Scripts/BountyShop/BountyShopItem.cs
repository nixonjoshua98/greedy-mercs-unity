using System.Numerics;

using SimpleJSON;

using UnityEngine;
using UnityEngine.UI;

namespace GreedyMercs.BountyShop.UI
{
    using GreedyMercs.BountyShop.Data;

    public class BountyShopItem : MonoBehaviour
    {
        [SerializeField] BountyShopItemID item;

        [Header("Components")]
        [SerializeField] Text stockText;
        [SerializeField] Text descriptionText;
        [SerializeField] Text purchaseCostText;
        [Space]
        [SerializeField] Button purchaseButton;

        void OnEnable()=> InvokeRepeating("UpdateUI", 0.0f, 0.25f);
        void OnDisable() => CancelInvoke("UpdateUI");

        void UpdateUI()
        {
            BountyItemState state = GameState.BountyShop.GetItem(item);
            BountyShopItemSO data = StaticData.BountyShop.GetItem(item);

            stockText.text          = string.Format("{0} Left in Stock", data.maxResetBuy - state.totalBought);
            descriptionText.text    = GetDescription();

            if (!GameState.BountyShop.IsValid)
                purchaseCostText.text = "...";

            else if (GameState.BountyShop.IsItemMaxBought(item))
                purchaseCostText.text = "SOLD OUT";

            else
                purchaseCostText.text = string.Format("Purchase\n{0} Points", data.PurchaseCost(state.totalBought));

            purchaseButton.interactable = GameState.BountyShop.IsValid && !GameState.BountyShop.IsItemMaxBought(item) && GameState.BountyShop.CanAffordItem(item);
        }

        protected void ProcessBoughtItem(JSONNode node)
        {
            switch (item)
            {
                case BountyShopItemID.PRESTIGE_POINTS:
                    GameState.Player.prestigePoints += BigInteger.Parse(node["prestigePointsReceived"].Value);
                    break;

                case BountyShopItemID.GEMS:
                    GameState.Player.gems += node["gemsReceived"].AsLong;
                    break;

                case BountyShopItemID.ARMOURY_POINTS:
                    GameState.Player.armouryPoints += node["armouryPointsReceived"].AsLong;
                    break;
            }
        }

        protected string GetDescription()
        {
            switch (item)
            {
                case BountyShopItemID.PRESTIGE_POINTS:
                    BountyShopItemSO data   = StaticData.BountyShop.GetItem(item);
                    BigInteger points       = StatsCache.GetPrestigePoints(Mathf.CeilToInt(GameState.LifetimeStats.maxPrestigeStage * data.GetFloat("maxStagePercent")));

                    return string.Format("{0} Runestones", Utils.Format.FormatNumber(points < 100 ? 100 : points));

                case BountyShopItemID.GEMS:
                    return string.Format("{0} Gems", StaticData.BountyShop.GetItem(item).GetLong("gems"));

                case BountyShopItemID.ARMOURY_POINTS:
                    return string.Format("{0} Armoury Points", StaticData.BountyShop.GetItem(item).GetLong("armouryPoints"));
            }

            return "Default Description";
        }


        public void OnClick()
        {
            BountyItemState state = GameState.BountyShop.GetItem(item);
            BountyShopItemSO data = StaticData.BountyShop.GetItem(item);

            bool inStock = data.maxResetBuy > state.totalBought;

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
