using System;

using UnityEngine;
using UnityEngine.UI;

namespace WeaponsUI
{
    using WeaponStaticData = WeaponData.WeaponStaticData;

    public class WeaponInfoPanel : MonoBehaviour
    {
        [Header("Text")]
        [SerializeField] Text titleText;
        [SerializeField] Text descText;
        [SerializeField] Text costText;
        [SerializeField] Text weaponDamageText;

        [Header("Images")]
        [SerializeField] Image mergeImage;
        [SerializeField] Image weaponImage;
        [SerializeField] Image characterImage;

        [Space]
        [SerializeField] Button buyButton;

        Action<Action> callback;

        WeaponStaticData weaponStaticData;

        Action UpdatePanel;

        public void Init(ScriptableCharacter chara, ScriptableWeapon weapon, int weaponIndex, Action<Action> func)
        {
            weaponStaticData = StaticData.Weapons.GetWeaponAtIndex(weaponIndex);

            callback = func;

            titleText.text  = chara.name;
            descText.text   = string.Format("Tier {0} Weapon", weaponStaticData.tier);

            weaponImage.sprite      = weapon.icon;
            characterImage.sprite   = chara.icon;

            UpdatePanel = () => { _UpdatePanel(chara, weapon, weaponIndex); };

            UpdatePanel();
        }

        void _UpdatePanel(ScriptableCharacter chara, ScriptableWeapon weapon, int weaponIndex)
        {
            int weaponsOwned = GameState.Weapons.Get(chara.character, weaponIndex);

            bool maxOwned = weaponsOwned >= weaponStaticData.maxOwned;

            weaponDamageText.text = Utils.Format.FormatNumber(Formulas.CalcWeaponDamage(weaponIndex, weaponsOwned) * 100) + "%";

            if (weaponIndex == 0)
            {
                mergeImage.sprite       = weapon.icon;
                costText.text           = string.Format("{0}x Bounty Points", weaponStaticData.buyCost);
                buyButton.interactable  = GameState.Player.bountyPoints >= weaponStaticData.buyCost && !maxOwned;
            }

            else
            {
                ScriptableWeapon prevWeapon = chara.weapons[weaponIndex - 1];
                WeaponStaticData prevWeaponStaticData = StaticData.Weapons.GetWeaponAtIndex(weaponIndex - 1);

                int prevWeaponOwned = GameState.Weapons.Get(chara.character, weaponIndex - 1);

                mergeImage.sprite = prevWeapon.icon;
                costText.text = string.Format("{0}/{1} Tier {2} Weapons", prevWeaponOwned, weaponStaticData.mergeCost, prevWeaponStaticData.tier);

                buyButton.interactable = prevWeaponOwned >= weaponStaticData.mergeCost && !maxOwned;
            }

            double currentDamageMultiplier = Formulas.CalcWeaponDamage(weaponIndex, weaponsOwned);
        }

        public void OnClick(int index)
        {
            buyButton.interactable = false;

            if (index == 0)
            {
                callback(UpdatePanel);
            }

            else
                Destroy(gameObject);
        }
    }
}