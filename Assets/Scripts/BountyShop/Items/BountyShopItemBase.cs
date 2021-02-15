using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GreedyMercs.BountyShop.UI
{
    using GreedyMercs.BountyShop.Data;

    public abstract class BountyShopItemBase : MonoBehaviour
    {
        [SerializeField] protected BountyShopItemID item;

        [Space]

        [SerializeField] protected Text purchaseCostText;
        [SerializeField] Button purchaseButton;

        [Space]

        [SerializeField, Range(0.0f, 1.0f)] float updateDelay = 0.25f;

        protected BountyShopItemData itemData => StaticData.BountyShop.Get(item);
        protected BountyItemState itemState => GameState.BountyShop.GetItem(item);

        void Start()
        {
            UpdateUI();
        }

        void OnEnable()
        {
            if (updateDelay > 0.0f)
                InvokeRepeating("UpdateUI", 0.0f, 0.25f);
        }

        void OnDisable() => CancelInvoke("UpdateUI");

        protected virtual void UpdatePurchaseText()
        {
            if (!GameState.BountyShop.IsValid)
                purchaseCostText.text = "...";

            else if (GameState.BountyShop.IsItemMaxBought(item))
                purchaseCostText.text = "SOLD OUT";

            else
            {
                int cost = GameState.BountyShop.NextPurchaseCost(item);

                purchaseCostText.text = cost == 0 ? "FREE" : cost.ToString();
            }
        }

        protected virtual void UpdateUI()
        {
            purchaseButton.interactable = GameState.BountyShop.CanPurchaseItem(item);

            UpdatePurchaseText();
        }
    }
}