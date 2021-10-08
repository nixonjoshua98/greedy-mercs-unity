using GM.BountyShop.Data;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace GM.Bounties
{
    public class BountyShopView : GM.UI.PanelController
    {
        [Header("Items")]
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
            InstantiateArmouryItems();
        }

        void InstantiateArmouryItems()
        {
            foreach (BountyShopArmouryItem item in App.Data.BountyShop.ArmouryItems)
            {
                GameObject o = Instantiate(ArmouryItemSlotObject, itemsParent);

                BountyShopArmouryItemSlot slot = o.GetComponent<BountyShopArmouryItemSlot>();

                slot.Setup(item.Id);

                itemSlots.Add(o);
            }
        }
    }
}
