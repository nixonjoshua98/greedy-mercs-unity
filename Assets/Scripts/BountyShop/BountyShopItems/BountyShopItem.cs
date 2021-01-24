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
        [Space]
        [SerializeField] Button purchaseButton;

        [Header("Prefabs")]
        [SerializeField] GameObject StockAlertObject;

        GameObject stockAlert;

        protected abstract void Awake();

        void Start()
        {
            if (GameState.BountyShop.IsItemMaxBought(item))
            {
                stockAlert = Utils.UI.Instantiate(StockAlertObject, transform, Vector3.zero);
            }
        }

        void OnEnable()
        {
            InvokeRepeating("UpdateUI", 0.0f, 0.1f);
        }

        void OnDisable()
        {
            CancelInvoke("UpdateUI");
        }

        void UpdateUI()
        {
            BountyItemState state = GameState.BountyShop.GetItem(item);
            BountyShopItemSO data = StaticData.BountyShop.GetItem(item);

            stockText.text          = string.Format("{0} Left in Stock", data.maxResetBuy - state.totalBought);
            purchaseCostText.text   = string.Format("Purchase\n{0} Points", data.PurchaseCost(state.totalBought));
            descriptionText.text    = GetDescription();

            purchaseButton.interactable = GameState.BountyShop.IsShopValid && data.maxResetBuy > state.totalBought;

            if (stockAlert != null && GameState.BountyShop.IsShopValid && !GameState.BountyShop.IsItemMaxBought(item))
                Destroy(stockAlert);
        }


        public void OnClick()
        {
            BountyItemState state = GameState.BountyShop.GetItem(item);
            BountyShopItemSO data = StaticData.BountyShop.GetItem(item);

            bool inStock = data.maxResetBuy > state.totalBought;

            if (GameState.BountyShop.IsShopValid && inStock)
            {
                JSONNode node = Utils.Json.GetDeviceNode();

                node.Add("itemId", (int)item);

                Server.BuyBountyShopItem(this, ServerCallback, node);
            }
        }

        void ServerCallback(long code, string compressed)
        {
            if (code == 200)
            {
                GameState.BountyShop.ProcessPurchase(item);

                if (GameState.BountyShop.IsItemMaxBought(item))
                {
                    stockAlert = Utils.UI.Instantiate(StockAlertObject, transform, Vector3.zero);
                }

                ProcessBoughtItem(Utils.Json.Decompress(compressed));
            }

            UpdateUI();
        }

        protected abstract void ProcessBoughtItem(JSONNode node);

        protected abstract string GetDescription();
    }
}
