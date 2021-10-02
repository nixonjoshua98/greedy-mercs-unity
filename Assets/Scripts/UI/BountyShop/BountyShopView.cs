using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using GM.BountyShop.Data;


namespace GM.Bounties
{
    public class BountyShopView : GM.UI.PanelController
    {
        [Header("Items")]
        [SerializeField] GameObject CurrencyItemSlotObject;
        [SerializeField] GameObject ArmouryItemSlotObject;
        [Space]
        [SerializeField] Transform itemsParent;

        [Header("References")]
        [SerializeField] TMPro.TMP_Text refreshText;

        List<GameObject> itemSlots;

        void Awake()
        {
            itemSlots = new List<GameObject>();

            UpdateRefreshText();
            InstantiateShopItems();
        }


        void FixedUpdate()
        {
            UpdateRefreshText();
        }


        void UpdateRefreshText()
        {
            TimeSpan timeUntilReset = App.Data.NextDailyReset - DateTime.UtcNow;

            refreshText.text = $"{FormatString.Seconds(timeUntilReset.TotalSeconds)}";
        }

        void InstantiateShopItems()
        {
            InstantiateItems();
            InstantiateArmouryItems();

            System.Random rng = new System.Random((int)(App.Data.NextDailyReset - DateTime.UtcNow).TotalDays);

            for (int i = 0; i < itemsParent.childCount; ++i)
            {
                Transform child = itemsParent.GetChild(i);

                child.SetSiblingIndex(rng.Next(0, itemsParent.childCount));
            }

        }

        void InstantiateItems()
        {
            foreach (BountyShopCurrencyItemData item in App.Data.BountyShop.CurrencyItem)
            {
                GameObject o = Instantiate(CurrencyItemSlotObject, itemsParent);

                BountyShopCurrencyItemSlot slot = o.GetComponent<BountyShopCurrencyItemSlot>();

                slot.Setup(item.Id);

                itemSlots.Add(o);
            }
        }

        void InstantiateArmouryItems()
        {
            foreach (BountyShopArmouryItemData item in App.Data.BountyShop.ArmouryItems)
            {
                GameObject o = Instantiate(ArmouryItemSlotObject, itemsParent);

                BountyShopArmouryItemSlot slot = o.GetComponent<BountyShopArmouryItemSlot>();

                slot.Setup(item.Id);

                itemSlots.Add(o);
            }
        }
    }
}
