using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Bounty
{
    using GM.Bounty;

    using CloseablePanel = GM.UI.CloseablePanel;

    public class BountyShopPanel : CloseablePanel
    {
        [SerializeField] GameObject bsItemObject;

        [SerializeField] Transform bsItemsParent;

        void Awake()
        {
            InstantiateItems();
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
