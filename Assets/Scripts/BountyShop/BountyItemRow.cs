using System.Numerics;

using SimpleJSON;

using UnityEngine;
using UnityEngine.UI;

namespace GreedyMercs.BountyShop.UI
{
    using GreedyMercs.BountyShop.Data;

    using Vector3 = UnityEngine.Vector3;

    public class BountyItemRow : MonoBehaviour
    {
        [SerializeField] BountyShopItemID itemId;

        [Header("Components")]
        [SerializeField] Text stockText;
        [SerializeField] Text descriptionText;
        [SerializeField] Text purchaseCostText;

        [Header("Prefabs")]
        [SerializeField] GameObject StockAlertObject;

        GameObject stockAlert;

        bool currentlyBuyingItem;

        void Awake()
        {
            currentlyBuyingItem = false;

            InvokeRepeating("UpdateUI", 0.0f, 0.1f);
        }

        void Start()
        {
            BountyShopItemSO data = StaticData.BountyShop.GetItem(itemId);

            descriptionText.text    = GetDescription();
            purchaseCostText.text   = string.Format("Purchase\n{0} Points", data.purchaseCost);
        }

        void UpdateUI()
        {
            PlayerBountyItem stateItem = GameState.BountyShop.GetItem(itemId);
            BountyShopItemSO dataItem = StaticData.BountyShop.GetItem(itemId);

            stockText.text = string.Format("{0} Left in Stock", dataItem.maxResetBuy - stateItem.totalBought);

            if (Formulas.Server.BountyShopNeedsRefresh || stateItem.totalBought >= dataItem.maxResetBuy)
            {
                stockAlert = Utils.UI.Instantiate(StockAlertObject, transform, Vector3.zero);

                CancelInvoke("UpdateUI");

                InvokeRepeating("CheckForNextRefresh", 0.0f, 0.1f);
            }
        }

        void CheckForNextRefresh()
        {
            PlayerBountyItem stateItem = GameState.BountyShop.GetItem(itemId);
            BountyShopItemSO dataItem = StaticData.BountyShop.GetItem(itemId);

            if (!Formulas.Server.BountyShopNeedsRefresh && stateItem.totalBought < dataItem.maxResetBuy)
            {
                CancelInvoke("CheckForNextStock");

                InvokeRepeating("UpdateUI", 0.0f, 0.1f);

                Destroy(stockAlert);
            }
        }


        public void OnClick()
        {
            PlayerBountyItem stateItem = GameState.BountyShop.GetItem(itemId);
            BountyShopItemSO dataItem = StaticData.BountyShop.GetItem(itemId);

            bool inStock = dataItem.maxResetBuy > stateItem.totalBought;

            if (!currentlyBuyingItem && inStock)
            {
                currentlyBuyingItem = true;

                JSONNode node = Utils.Json.GetDeviceNode();

                node.Add("itemId", (int)itemId);

                Server.BuyBountyShopItem(this, ServerCallback, node);
            }
        }

        void ServerCallback(long code, string compressed)
        {
            currentlyBuyingItem = false;

            if (code == 200)
            {
                PlayerBountyItem state  = GameState.BountyShop.GetItem(itemId);
                BountyShopItemSO data   = StaticData.BountyShop.GetItem(itemId);

                state.totalBought++;

                GameState.Player.bountyPoints -= data.purchaseCost;

                ProcessBoughtItem(Utils.Json.Decompress(compressed));
            }
        }

        void ProcessBoughtItem(JSONNode node)
        {
            switch (itemId)
            {
                case BountyShopItemID.PRESTIGE_90:
                    GameState.Player.prestigePoints += BigInteger.Parse(node["receivedPrestigePoints"].Value);
                    break;
            }
        }

        string GetDescription()
        {
            switch (itemId)
            {
                case BountyShopItemID.PRESTIGE_90:
                    return string.Format("{0} Combat Experience", Utils.Format.FormatNumber(StatsCache.GetPrestigePoints(Mathf.CeilToInt(GameState.Player.maxPrestigeStage * 0.9f))));
            }

            return "<missing>";
        }
    }
}
