using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GreedyMercs.Armoury.UI
{
    using GreedyMercs.Armoury.Data;

    public class ArmouryPanel : MonoBehaviour
    {
        [Header("Compoents")]
        [SerializeField] Text damageBonusText;
        [SerializeField] Text weaponPointText;

        [SerializeField] Transform itemsParent;

        [Header("Prefabs")]
        [SerializeField] GameObject ArmouryItemObject;
        [SerializeField] GameObject ArmouryPanelObject;

        // ===
        List<ArmouryItemSlot> slots;

        void OnEnable()
        {
            slots = new List<ArmouryItemSlot>();

            Populate();

            InvokeRepeating("UpdateUI", 0.0f, 0.25f);
        }

        void OnDisable()
        {
            CancelInvoke("UpdateUI");

            for (int i = 0; i < itemsParent.childCount; ++i) Destroy(itemsParent.GetChild(i).gameObject);
        }

        void UpdateUI()
        {
            BigDouble dmg = StatsCache.ArmouryDamageMultiplier == 1.0 ? 0 : StatsCache.ArmouryDamageMultiplier;

            damageBonusText.text = string.Format("{0}% Bonus Mercenary Damage", Utils.Format.FormatNumber(dmg * 100));
        }

        void FixedUpdate()
        {
            weaponPointText.text = GameState.Player.armouryPoints.ToString();
        }

        void Populate()
        {
            foreach (ArmouryItemSO w in StaticData.Armoury.GetAllItems())
            {
                ArmouryItemSlot slot = Utils.UI.Instantiate(ArmouryItemObject, itemsParent, Vector3.zero).GetComponent<ArmouryItemSlot>();

                slot.Init(w);

                slot.SetButtonCallback(() => { OnIconClick(w); });

                slots.Add(slot);
            }
        }

        // === Button Callback ===

        void OnIconClick(ArmouryItemSO item)
        {
            ArmouryWeaponPanel panel = Utils.UI.Instantiate(ArmouryPanelObject, Vector3.zero).GetComponent<ArmouryWeaponPanel>();

            panel.Init(item);
        }
    }
}