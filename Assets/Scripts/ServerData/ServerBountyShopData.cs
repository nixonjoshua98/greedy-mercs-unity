﻿using System;
using System.Collections;
using System.Collections.Generic;


namespace GM.BountyShop
{
    using SimpleJSON;

    public enum BountyShopItemType
    {
        // Represents the type of item in the shop
        // The shop could contain multiple items (with unique ids) but multiple of the same item type
        FLAT_DIAMONDS = 100,
    }

    public class BountyShopItem
    {
        public int ID;

        public BountyShopItemType ItemType;

        public BountyShopItem(int id, JSONNode node)
        {
            ID = id;

            ItemType = (BountyShopItemType)id; // Quick conversion
        }
    }


    public class ServerBountyShopData
    {
        Dictionary<int, BountyShopItem> items;

        public ServerBountyShopData(JSONNode node)
        {
            SetupItems(node);
        }


        void SetupItems(JSONNode node)
        {
            items = new Dictionary<int, BountyShopItem>();

            node = node["items"];

            foreach (string key in node.Keys)
            {
                int id = int.Parse(key);

                items.Add(id, new BountyShopItem(id, node[key]));
            }
        }
    }
}