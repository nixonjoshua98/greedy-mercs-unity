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

        [Header("Weapon Parents")]
        [SerializeField] Transform swordParent;
        [SerializeField] Transform staffParent;
        [SerializeField] Transform axesParent;

        [Header("Prefabs")]
        [SerializeField] GameObject ArmouryWeaponObject;

        // ===
        List<ArmouryWeaponSlot> slots;

        void OnEnable()
        {
            slots = new List<ArmouryWeaponSlot>();

            Populate(swordParent, StaticData.Armoury.GetWeapons(WeaponType.SWORD));
            Populate(staffParent, StaticData.Armoury.GetWeapons(WeaponType.STAFF));
            Populate(axesParent, StaticData.Armoury.GetWeapons(WeaponType.AXE));

            InvokeRepeating("UpdateUI", 0.0f, 0.25f);
        }

        void OnDisable()
        {
            CancelInvoke("UpdateUI");

            for (int i = 0; i < staffParent.childCount; ++i) Destroy(staffParent.GetChild(i).gameObject);
            for (int i = 0; i < swordParent.childCount; ++i) Destroy(swordParent.GetChild(i).gameObject);
            for (int i = 0; i < axesParent.childCount; ++i) Destroy(axesParent.GetChild(i).gameObject);
        }

        void UpdateUI()
        {
            foreach (ArmouryWeaponSlot entry in slots)
            {
                entry.UpdateUI();
            }

            BigDouble dmg = StatsCache.ArmouryDamageMultiplier == 1.0 ? 0 : StatsCache.ArmouryDamageMultiplier;

            damageBonusText.text = string.Format("{0}% Bonus Mercenary Damage", Utils.Format.FormatNumber(dmg * 100));
        }

        void FixedUpdate()
        {
            weaponPointText.text = GameState.Player.weaponPoints.ToString();
        }

        void Populate(Transform parent, List<ArmouryItemSO> weapons)
        {
            foreach (ArmouryItemSO w in weapons)
            {
                if (GameState.Armoury.GetWeapon(w.Index).level > 0)
                {
                    ArmouryWeaponSlot slot = Utils.UI.Instantiate(ArmouryWeaponObject, parent, Vector3.zero).GetComponent<ArmouryWeaponSlot>();

                    slot.Init(w);

                    slots.Add(slot);
                }
            }
        }
    }
}