using UnityEngine;
using System;
using UnityEngine.UI;


namespace GM.Bounty
{
    using GM.Data;

    using PanelController = GM.UI.PanelController;

    public class BountyShopPanel : PanelController
    {
        [Header("Items")]
        [SerializeField] GameObject bsItemObject;
        [SerializeField] Transform bsItemsParent;

        [Header("Armoury Items")]
        [SerializeField] GameObject bsArmouryItemObject;
        [SerializeField] Transform bsArmouryItemsParent;

        [Header("Elements")]
        [SerializeField] Text refreshText;

        void Awake()
        {
            UpdateRefreshText();
            
            InstantiateItems();
            InstantiateArmouryItems();
        }


        protected override void PeriodicUpdate()
        {
            UpdateRefreshText();
        }


        void UpdateRefreshText()
        {
            TimeSpan timeUntilReset = GameData.Get.NextDailyReset - DateTime.UtcNow;

            refreshText.text = $"{FormatString.Seconds(timeUntilReset.TotalSeconds)}";
        }


        void InstantiateItems()
        {
            foreach (BountyShopItem item in UserData.Get.BountyShop.Items)
            {
                GameObject o = Instantiate(bsItemObject, bsItemsParent);

                BSItemSlot slot = o.GetComponent<BSItemSlot>();

                slot.Setup(item.ID);
            }
        }


        void InstantiateArmouryItems()
        {
            foreach (BountyShopArmouryItem item in UserData.Get.BountyShop.ArmouryItems)
            {
                GameObject o = Instantiate(bsArmouryItemObject, bsArmouryItemsParent);

                BsArmouryItemSlot slot = o.GetComponent<BsArmouryItemSlot>();

                slot.Setup(item.ID);
            }
        }
    }
}