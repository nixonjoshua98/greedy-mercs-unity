using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GreedyMercs.Armoury.UI
{
    using GreedyMercs.Armoury.Data;

    public class ArmouryWeaponPanel : MonoBehaviour
    {
        [Header("Components (Weapon)")]
        [SerializeField] Image colouredWeapon;
        [SerializeField] Image shadowWeapon;

        [Header("Components (Level + Damage)")]
        [SerializeField] Text levelText;
        [SerializeField] Text damageText;

        ArmouryItemSO weaponItem;

        public void Init(ArmouryItemSO item)
        {
            weaponItem = item;

            Utils.UI.ScaleImageW(colouredWeapon, item.icon, 400.0f);
            Utils.UI.ScaleImageW(shadowWeapon, item.icon, 400.0f);

            UpdateUI();
        }

        void UpdateUI()
        {

            string Stringify(double d) => Utils.Format.FormatNumber(d * 100) + "%";

            ArmouryWeaponState state = GameState.Armoury.GetWeapon(weaponItem.Index);

            levelText.text  = string.Format("Level {0} -> {1}", state.level, state.level + 1);

            damageText.text = string.Format("Damage {0} -> {1}", 
                Stringify(GameState.Armoury.DamageBonus(weaponItem.Index)),
                Stringify(GameState.Armoury.DamageBonus(weaponItem.Index, state.level + 1))
                );

        }
    }
}
