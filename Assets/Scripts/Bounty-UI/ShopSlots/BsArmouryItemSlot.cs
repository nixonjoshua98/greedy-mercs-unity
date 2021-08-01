using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GM.BountyShop
{
    using GM.Data;
    using GM.UI;
    using GM.Bounty;


    public class BsArmouryItemSlot : AbstractBountyShopSlot
    {
        [Header("Components - Scripts")]
        [SerializeField] StarRatingController starController;

        ArmouryItemData ArmouryItemData { get { return GameData.Get.Armoury.Get(ShopItemData.ArmouryItemID); } }
        protected new BountyShopArmouryItem ShopItemData => UserData.Get.BountyShop.GetArmouryItem(_itemId);


        protected override void OnItemAssigned()
        {
            base.OnItemAssigned();

            starController.Show(ArmouryItemData.Tier);
        }

        void FixedUpdate()
        {
            if (!_isUpdatingUi)
                return;

            outStockObject.SetActive(!UserData.Get.BountyShop.InStock(ShopItemData.ID));

            purchaseCostText.text = ShopItemData.PurchaseCost.ToString();
        }


        // = = = Button Callbacks ===
        public void OnPurchaseButton()
        {
            UserData.Get.BountyShop.PurchaseArmouryItem(ShopItemData.ID, (_) => { });
        }
    }
}