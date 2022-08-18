using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SRC.UI;
using SRC.BountyShop.Data;

namespace SRC.Bounties.Shop.UI
{
    public class BountyShopListSlot : SRC.Core.GMMonoBehaviour
    {
        [SerializeField] RarityIcon Icon;
        [Space]
        [SerializeField] TMP_Text NameText;
        [SerializeField] TMP_Text DescriptionText;

        [Header("Buy Button")]
        [SerializeField] Button BuyButton;

        BountyShopArmouryItem ShopItem;

        public void Initialize(BountyShopArmouryItem aItem)
        {
            ShopItem = aItem;

            UpdateUI();
        }

        void UpdateUI()
        {
            Icon.Initialize(ShopItem.Item);

            NameText.text = $"<color=orange>[Armoury]</color> {ShopItem.Item.Name}";
            BuyButton.interactable = ShopItem.InStock;
        }

        private void FixedUpdate()
        {
            
        }

        public void BuyItem()
        {
            App.BountyShop.PurchaseArmouryItem(ShopItem.ID, (success, resp) =>
            {
                
            });
        }
    }
}
