using SimpleJSON;

using UnityEngine;
using UnityEngine.UI;

namespace GreedyMercs.BountyShop.UI
{
    using GreedyMercs.BountyShop.Data;

    using Vector3 = UnityEngine.Vector3;

    public abstract class BountyShopItem : MonoBehaviour
    {
        protected BountyShopItemID item;

        [Header("Components")]
        [SerializeField] Text stockText;
        [SerializeField] Text descriptionText;
        [SerializeField] Text purchaseCostText;

        [Header("Prefabs")]
        [SerializeField] GameObject StockAlertObject;

        GameObject stockAlert;

        bool currentlyBuyingItem;

        protected abstract void Awake();

        void Start()
        {
            currentlyBuyingItem = false;

            UpdateUI();
        }

        void OnEnable()
        {
            InvokeRepeating("UpdateUI", 0.0f, 0.1f);
        }

        void OnDisable()
        {
            CancelInvoke("UpdateUI");

            CancelInvoke("CheckForNextRefresh");
        }

        void UpdateUI()
        {
            BountyItemState state = GameState.BountyShop.GetItem(item);
            BountyShopItemSO data = StaticData.BountyShop.GetItem(item);

            stockText.text          = string.Format("{0} Left in Stock", data.maxResetBuy - state.totalBought);
            purchaseCostText.text   = string.Format("Purchase\n{0} Points", data.PurchaseCost(state.totalBought));
            descriptionText.text    = GetDescription();

            if (Formulas.Server.BountyShopNeedsRefresh || state.totalBought >= data.maxResetBuy)
            {
                stockAlert = Utils.UI.Instantiate(StockAlertObject, transform, Vector3.zero);

                CancelInvoke("UpdateUI");

                InvokeRepeating("CheckForNextRefresh", 0.0f, 0.1f);
            }
        }

        void CheckForNextRefresh()
        {
            BountyItemState state   = GameState.BountyShop.GetItem(item);
            BountyShopItemSO data   = StaticData.BountyShop.GetItem(item);

            if (!Formulas.Server.BountyShopNeedsRefresh && state.totalBought < data.maxResetBuy)
            {
                CancelInvoke("CheckForNextStock");

                InvokeRepeating("UpdateUI", 0.0f, 0.1f);

                Destroy(stockAlert);
            }
        }


        public void OnClick()
        {
            BountyItemState stateItem = GameState.BountyShop.GetItem(item);
            BountyShopItemSO dataItem = StaticData.BountyShop.GetItem(item);

            bool inStock = dataItem.maxResetBuy > stateItem.totalBought;

            if (!currentlyBuyingItem && inStock)
            {
                currentlyBuyingItem = true;

                JSONNode node = Utils.Json.GetDeviceNode();

                node.Add("itemId", (int)item);

                Server.BuyBountyShopItem(this, ServerCallback, node);
            }

            UpdateUI();
        }

        void ServerCallback(long code, string compressed)
        {
            currentlyBuyingItem = false;

            if (code == 200)
            {
                BountyItemState state  = GameState.BountyShop.GetItem(item);
                BountyShopItemSO data   = StaticData.BountyShop.GetItem(item);

                state.totalBought++;

                GameState.Player.bountyPoints -= data.purchaseCost;

                ProcessBoughtItem(Utils.Json.Decompress(compressed));
            }
        }

        protected abstract void ProcessBoughtItem(JSONNode node);

        protected abstract string GetDescription();
    }
}
