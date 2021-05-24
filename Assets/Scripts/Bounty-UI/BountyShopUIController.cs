using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GM.BountyShop
{
    using StaticData = GreedyMercs.StaticData;

    public class BountyShopUIController : MonoBehaviour
    {
        [Header("Prefabs - UI")]
        [SerializeField] GameObject ItemSlotObject;
        [SerializeField] GameObject ArmouryItemSlot;

        [Header("Components - UI")]
        [SerializeField] Transform ItemsParent;
        [Space]
        [SerializeField] Text shopRefreshText;

        List<GameObject> items;

        void Awake()
        {
            items = new List<GameObject>();
        }

        void OnEnable()
        {
            BountyShopManager.Instance.Refresh(() => { InstantiateItemSlots(); });
        }

        void FixedUpdate()
        {
            TimeSpan timeUntilReset = Funcs.TimeUntil(StaticData.NextDailyReset);

            shopRefreshText.text = string.Format("Refreshes in {0}", Funcs.Format.Seconds(timeUntilReset.TotalSeconds));
        }

        void InstantiateItemSlots()
        {
            DestroyAllSlots();

            foreach (AbstractBountyShopData itemData in BountyShopManager.Instance.ServerData.NormalItems)
            {
                items.Add(InstantiateSlot(ItemSlotObject, itemData));
            }

            foreach (AbstractBountyShopData itemData in BountyShopManager.Instance.ServerData.ArmouryItems)
            {
                items.Add(InstantiateSlot(ArmouryItemSlot, itemData));
            }
        }

        GameObject InstantiateSlot(GameObject prefab, AbstractBountyShopData itemData)
        {
            GameObject o = Funcs.UI.Instantiate(prefab, ItemsParent);

            AbstractBountyShopSlot slot = o.GetComponent<AbstractBountyShopSlot>();

            slot.SetID(itemData.ID);

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