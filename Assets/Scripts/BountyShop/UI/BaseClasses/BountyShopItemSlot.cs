using SRC.BountyShop.Data;
using SRC.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SRC.BountyShop.UI.BaseClasses
{
    public abstract class BountyShopItemSlot<T> : SRC.Core.GMMonoBehaviour where T : BountyShopItem
    {
        [SerializeField] protected RarityIcon Icon;
        [Space]
        [SerializeField] protected TMP_Text NameText;
        [SerializeField] protected TMP_Text DescriptionText;
        [SerializeField] protected TMP_Text PurchaseCostText;

        [Header("Buy Button")]
        [SerializeField] protected Button BuyButton;

        protected T ShopItem;

        public void Initialize(T aItem)
        {
            ShopItem = aItem;

            UpdateSlotUI();
        }

        protected void UpdateSlotUI()
        {
            BuyButton.interactable = ShopItem.InStock;
            PurchaseCostText.text = ShopItem.PurchaseCost.ToString();

            UpdateUI();
        }

        protected abstract void UpdateUI();

        public abstract void BuyItem();
    }
}
