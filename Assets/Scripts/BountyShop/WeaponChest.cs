using System.Collections;
using System.Collections.Generic;

using SimpleJSON;

using UnityEngine;
using UnityEngine.UI;

namespace GreedyMercs.Armoury.UI
{
    using GreedyMercs.BountyShop.Data;

    public class WeaponChest : MonoBehaviour
    {
        BountyShopItemID item = BountyShopItemID.WEAPON_CHEST;

        [Header("Components")]
        [SerializeField] Text purchaseCostText;
        [Space]
        [SerializeField] Button purchaseButton;

        void OnEnable() => InvokeRepeating("UpdateUI", 0.0f, 0.25f);
        void OnDisable() => CancelInvoke("UpdateUI");

        void Start()
        {
            if (GameState.BountyShop.IsItemMaxBought(item))
                purchaseCostText.text = "SOLD OUT";

            UpdateUI();
        }

        void UpdateUI()
        {
            BountyItemState state = GameState.BountyShop.GetItem(item);
            BountyShopItemSO data = StaticData.BountyShop.GetItem(item);

            purchaseButton.interactable = GameState.BountyShop.IsValid && !GameState.BountyShop.IsItemMaxBought(item) && GameState.BountyShop.CanAffordItem(item);

            if (!GameState.BountyShop.IsValid)
                purchaseCostText.text = "...";

            else if (GameState.BountyShop.IsItemMaxBought(item))
                purchaseCostText.text = "SOLD OUT";

            else
                purchaseCostText.text = string.Format("Purchase\n{0} Points", data.PurchaseCost(state.totalBought));
        }

        protected void ProcessBoughtItem(JSONNode node)
        {
            GameState.Armoury.AddItem(node["itemReceived"].AsInt);
        }

        public void PurchaseChest()
        {
            void Purchase()
            {
                JSONNode node = Utils.Json.GetDeviceNode();

                node.Add("itemId", (int)item);

                Server.BuyBountyShopItem(ServerCallback, node);
            }

            BountyItemState state = GameState.BountyShop.GetItem(item);
            BountyShopItemSO data = StaticData.BountyShop.GetItem(item);

            bool inStock = data.maxResetBuy > state.totalBought;

            if (GameState.BountyShop.IsValid && inStock)
            {
                Utils.UI.ShowYesNoPrompt("Purchase Chest?", Purchase);
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