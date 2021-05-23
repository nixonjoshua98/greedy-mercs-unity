using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GM.BountyShop
{
    using ServerData = GreedyMercs.StaticData;

    public class BountyShopSection : MonoBehaviour
    {
        [Header("Prefabs - UI")]
        [SerializeField] GameObject ItemSlotObject;

        [Header("Transforms - UI")]
        [SerializeField] Transform ItemsParent;

        List<GameObject> items;

        void Awake()
        {
            items = new List<GameObject>();
        }

        // = = = Callbacks = = =
        public void OnShopRefreshed()
        {
            InstantiateItemSlots();
        }

        // = = =
        void InstantiateItemSlots()
        {
            DestroyAllSlots();

            foreach (AbstractBountyShopData itemData in BountyShopManager.Instance.ServerData.All)
            {
                GameObject o = Funcs.UI.Instantiate(ItemSlotObject, ItemsParent);

                AbstractBountyShopSlot slot = o.GetComponent<AbstractBountyShopSlot>();

                slot.SetID(itemData.ID);

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