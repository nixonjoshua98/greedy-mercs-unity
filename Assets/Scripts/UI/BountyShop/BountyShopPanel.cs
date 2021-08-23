using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;


namespace GM.Bounty
{
    using PanelController = GM.UI.PanelController;

    public class BountyShopPanel : PanelController
    {
        [Header("Items")]
        [SerializeField] GameObject bsItemObject;
        [SerializeField] GameObject bsArmouryItemObject;

        [SerializeField] Transform itemsParent;

        [Header("Elements")]
        [SerializeField] Text refreshText;

        List<GameObject> itemSlots;

        void Awake()
        {
            itemSlots = new List<GameObject>();

            UpdateRefreshText();

            InstantiateShopItems();
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

        void InstantiateShopItems()
        {
            InstantiateItems();
            InstantiateArmouryItems();

            System.Random rng = new System.Random((int)(GameData.Get.NextDailyReset - DateTime.UtcNow).TotalDays);

            for (int i = 0; i < itemsParent.childCount; ++i)
            {
                Transform child = itemsParent.GetChild(i);

                child.SetSiblingIndex(rng.Next(0, itemsParent.childCount));
            }

        }


        void InstantiateItems()
        {
            foreach (BountyShopItem item in UserData.Get.BountyShop.Items)
            {
                GameObject o = Instantiate(bsItemObject, itemsParent);

                BountyShopCurrencyItemSlot slot = o.GetComponent<BountyShopCurrencyItemSlot>();

                slot.Setup(item.ID);
                itemSlots.Add(o);
            }
        }


        void InstantiateArmouryItems()
        {
            foreach (BountyShopArmouryItem item in UserData.Get.BountyShop.ArmouryItems)
            {
                GameObject o = Instantiate(bsArmouryItemObject, itemsParent);

                BountyShopArmouryItemSlot slot = o.GetComponent<BountyShopArmouryItemSlot>();

                slot.Setup(item.ID);
                itemSlots.Add(o);
            }
        }
    }
}
