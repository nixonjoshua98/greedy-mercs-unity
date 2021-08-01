using UnityEngine;
using System;


namespace GM.Bounty
{
    using TMPro;

    using GM.Data;

    using CloseablePanel = GM.UI.CloseablePanel;

    public class BountyShopPanel : CloseablePanel
    {
        [SerializeField] GameObject bsItemObject;

        [SerializeField] Transform bsItemsParent;

        [Header("Elements")]
        [SerializeField] TMP_Text refreshText;

        void Awake()
        {
            InstantiateItems();
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
    }
}
