﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GM.BountyShop
{
    using ServerData = GreedyMercs.StaticData;

    public class BSItemsSection : MonoBehaviour
    {
        [Header("Prefabs - UI")]
        [SerializeField] GameObject ItemSlotObject;

        [Header("Transforms - UI")]
        [SerializeField] Transform ItemsParent;

        [Header("Components - UI")]
        [SerializeField] Text shopRefreshText;

        List<GameObject> items;

        void Awake()
        {
            items = new List<GameObject>();
        }

        void OnEnable()
        {
            BountyShopManager.Instance.Refresh(OnShopRefreshed);
        }

        void FixedUpdate()
        {
            shopRefreshText.text = "Refreshes in ???";
        }

        // = = = Server Callbacks = = =
        void OnShopRefreshed()
        {
            InstantiateItemSlots();
        }

        // = = =
        void InstantiateItemSlots()
        {
            DestroyAllSlots();

            foreach (BountyShopItem itemData in ServerData.BountyShop.Items)
            {
                GameObject o = Funcs.UI.Instantiate(ItemSlotObject, ItemsParent);

                BountyShopItemSlot slot = o.GetComponent<BountyShopItemSlot>();

                slot.SetItem(itemData);

                items.Add(o);
            }
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