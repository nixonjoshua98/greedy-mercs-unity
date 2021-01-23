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

        [Header("Prefabs")]
        [SerializeField] GameObject ArmouryWeaponObject;

        // ===
        Dictionary<int, ArmouryWeaponSlot> slots;

        void Awake()
        {
            slots = new Dictionary<int, ArmouryWeaponSlot>();
        }

        void Start()
        {
            Populate(swordParent, StaticData.Armoury.GetWeapons(WeaponType.SWORD));
            Populate(staffParent, StaticData.Armoury.GetWeapons(WeaponType.STAFF));

            UpdateUI();
        }

        void OnEnable()
        {
            UpdateUI();
        }

        void UpdateUI()
        {
            foreach (var entry in slots)
            {
                ArmouryWeaponState state = GameState.Armoury.GetWeapon(entry.Key);

                entry.Value.levelText.text = state.level.ToString();
            }

            damageBonusText.text = string.Format("{0}% Bonus Mercenary Damage", Utils.Format.FormatNumber(GameState.Armoury.DamageBonus() * 100));
        }

        void FixedUpdate()
        {
            weaponPointText.text = GameState.Player.weaponPoints.ToString();
        }

        void Populate(Transform parent, List<ArmouryItemSO> weapons)
        {
            foreach (ArmouryItemSO w in weapons)
            {
                ArmouryWeaponSlot slot = Utils.UI.Instantiate(ArmouryWeaponObject, parent, Vector3.zero).GetComponent<ArmouryWeaponSlot>();

                slots.Add(w.Index, slot);

                Utils.UI.ScaleImageW(slot.iconImage, w.icon, 150.0f);
            }
        }
    }
}