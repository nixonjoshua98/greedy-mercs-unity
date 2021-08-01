using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;


namespace GM.BountyShop
{
    using GM.Data;

    public class BountyShopUIController : MonoBehaviour
    {
        [Header("Prefabs - UI")]
        [SerializeField] GameObject ItemSlotObject;
        [SerializeField] GameObject ArmouryItemSlot;

        [Header("Components - UI")]
        [SerializeField] Transform normalItemsParent;
        [SerializeField] Transform armouryItemsParent;
        [Space]
        [SerializeField] Text shopRefreshText;

        List<GameObject> items;

        void Awake()
        {
            items = new List<GameObject>();
        }

        void Start()
        {
            InstantiateItemSlots();
        }


        void FixedUpdate()
        {
            TimeSpan timeUntilReset = StaticData.NextDailyReset - DateTime.UtcNow;

            shopRefreshText.text = string.Format("Refreshes in {0}", FormatString.Seconds(timeUntilReset.TotalSeconds));
        }

        void InstantiateItemSlots()
        {
            DestroyAllSlots();

            UserBountyShop shop = UserData.Get.BountyShop;

            foreach (BountyShopItem itemData in shop.ServerData.Items)
            {
                items.Add(InstantiateSlot(ItemSlotObject, normalItemsParent, itemData));
            }

            foreach (BountyShopArmouryItem itemData in shop.ServerData.ArmouryItems)
                items.Add(InstantiateSlot(ArmouryItemSlot, armouryItemsParent, itemData));
        }

        GameObject InstantiateSlot(GameObject prefab, Transform parent, AbstractBountyShopItem itemData)
        {
            GameObject o = CanvasUtils.Instantiate(prefab, parent);

            AbstractBountyShopSlot slot = o.GetComponent<AbstractBountyShopSlot>();

            slot.Setup(itemData.ID);

            return o;
        }

        void DestroyAllSlots()
        {
            foreach (GameObject o in items)
            {
                Destroy(o);
            }

            items.Clear();
        }
    }
}