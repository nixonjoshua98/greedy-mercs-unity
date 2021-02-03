using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GreedyMercs.Armoury.UI
{
    using GreedyMercs.Armoury.Data;

    public class ArmouryWeaponSlot : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] Image iconImage;
        [SerializeField] Text levelText;

        [Header("Prefabs")]
        [SerializeField] GameObject ArmouryWeaponPanelObject;

        ArmouryItemSO weaponItem;

        public void Init(ArmouryItemSO weapon)
        {
            weaponItem = weapon;

            iconImage.sprite = weapon.icon;
        }

        public void UpdateUI()
        {
            ArmouryWeaponState state = GameState.Armoury.GetWeapon(weaponItem.Index);

            levelText.text = state.level.ToString();
        }

        public void OnClick()
        {
            ArmouryWeaponPanel panel = Utils.UI.Instantiate(ArmouryWeaponPanelObject, Vector3.zero).GetComponent<ArmouryWeaponPanel>();

            panel.Init(weaponItem);
        }
    }
}