using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using GM.Armoury;

namespace GreedyMercs.Armoury.UI
{
    using GM.Armoury;
    using GM.Inventory;

    public class ArmouryPanel : MonoBehaviour
    {
        [Header("Compoents")]
        [SerializeField] Text damageBonusText;
        [SerializeField] Text weaponPointText;

        [SerializeField] Transform itemsParent;

        [Header("Prefabs")]
        [SerializeField] GameObject ArmouryItemObject;
        [SerializeField] GameObject ItemPopupObject;

        // ===
        Dictionary<int, ArmouryItem> itemObjects;

        void Awake()
        {
            itemObjects = new Dictionary<int, ArmouryItem>();
        }

        void OnEnable()
        {
            foreach (ArmouryItemState state in ArmouryManager.Instance.GetOwned())
            {
                if (!itemObjects.ContainsKey(state.ItemID))
                {
                    InstantiateItem(state);
                }
            }
        }

        void FixedUpdate()
        {
            InventoryManager inv = InventoryManager.Instance;

            weaponPointText.text = inv.ArmouryPoints.ToString();

            BigDouble dmg = StatsCache.ArmouryDamageMultiplier == 1.0 ? 0 : StatsCache.ArmouryDamageMultiplier;

            damageBonusText.text = string.Format("{0}% Mercenary Damage", Utils.Format.FormatNumber(dmg * 100));
        }

        void InstantiateItem(ArmouryItemState state)
        {
            ArmouryItemData serverItemData = StaticData.Armoury.Get(state.ItemID);

            ArmouryItem item = Utils.UI.Instantiate(ArmouryItemObject, itemsParent, Vector3.zero).GetComponent<ArmouryItem>();

            item.Init(serverItemData);

            item.SetButtonCallback(() => { OnIconClick(serverItemData); });

            itemObjects[state.ItemID] = item;
        }

        // === Button Callback ===

        void OnIconClick(ArmouryItemData item)
        {
            GameObject obj = Funcs.UI.Instantiate(ItemPopupObject);

            ArmouryItemPopup panel = obj.GetComponent<ArmouryItemPopup>();

            panel.Init(item);
        }
    }
}