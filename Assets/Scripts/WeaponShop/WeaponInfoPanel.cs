using System;
using System.Numerics;

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
        [SerializeField] Text buyMaxText;
        [SerializeField] Text weaponDamageText;

        [Header("Images")]
        [SerializeField] Image mergeImage;
        [SerializeField] Image weaponImage;
        [SerializeField] Image characterImage;

        [Space]
        [SerializeField] Button buyButton;
        [SerializeField] Button buyMaxButton;

        Action<int, Action> callback;

        WeaponStaticData weaponStaticData;

        // - Scriptables
        ScriptableCharacter character;
        ScriptableWeapon weapon;

        int weaponIndex;

        int BuyMaxAmount
        {
            get
            {
                int weaponsOwned = GameState.Weapons.Get(character.character, weaponIndex);
                int weaponsLeft = weaponStaticData.maxOwned - weaponsOwned;

                if (weaponIndex == 0)
                    return (int)BigInteger.Min(weaponsLeft, GameState.Player.bountyPoints / weaponStaticData.buyCost);

                else
                {
                    int prevWeaponOwned = GameState.Weapons.Get(character.character, weaponIndex - 1);

                    return Mathf.Min(weaponsLeft, prevWeaponOwned / weaponStaticData.mergeCost);
                }
            }
        }

        public void Init(ScriptableCharacter chara, ScriptableWeapon _weapon, int _weaponIndex, Action<int, Action> func)
        {
            callback    = func;
            character   = chara;
            weapon      = _weapon;
            weaponIndex = _weaponIndex;

            weaponStaticData = StaticData.Weapons.GetWeaponAtIndex(weaponIndex);

            titleText.text  = chara.name;
            descText.text   = string.Format("Tier {0} Weapon", weaponStaticData.tier);

            weaponImage.sprite      = weapon.icon;
            characterImage.sprite   = chara.icon;

            UpdatePanel();
        }

        void UpdatePanel()
        {
            int weaponsOwned = GameState.Weapons.Get(character.character, weaponIndex);

            weaponDamageText.text   = Utils.Format.FormatNumber(Formulas.CalcWeaponDamage(weaponIndex, weaponsOwned) * 100) + "%";
            buyMaxText.text         = "Buy x" + BuyMaxAmount;

            if (weaponIndex == 0)
                UpdateBuyWeapon();

            else
                UpdateMergeWeapon();
        }

        void UpdateBuyWeapon()
        {
            int weaponsOwned    = GameState.Weapons.Get(character.character, weaponIndex);
            int weaponsLeft     = weaponStaticData.maxOwned - weaponsOwned;

            mergeImage.sprite   = weapon.icon;
            costText.text       = string.Format("{0}x Bounty Points", weaponStaticData.buyCost);

            buyButton.interactable      = weaponsLeft > 0 && GameState.Player.bountyPoints >= (weaponStaticData.buyCost * 1);
            buyMaxButton.interactable   = weaponsLeft > 0 && GameState.Player.bountyPoints >= (weaponStaticData.buyCost * BuyMaxAmount);
        }

        void UpdateMergeWeapon()
        {
            int weaponsOwned    = GameState.Weapons.Get(character.character, weaponIndex);
            int weaponsLeft     = weaponStaticData.maxOwned - weaponsOwned;

            ScriptableWeapon prevWeapon = character.weapons[weaponIndex - 1];

            int prevWeaponOwned = GameState.Weapons.Get(character.character, weaponIndex - 1);

            mergeImage.sprite   = prevWeapon.icon;
            costText.text       = string.Format("{0}/{1} Tier {2} Weapons", prevWeaponOwned, weaponStaticData.mergeCost, weaponIndex + 1);

            buyButton.interactable      = weaponsLeft > 0 && prevWeaponOwned >= (weaponStaticData.mergeCost * 1);
            buyMaxButton.interactable   = weaponsLeft > 0 && prevWeaponOwned >= (weaponStaticData.mergeCost * BuyMaxAmount);
        }

        public void OnClose()
        {
            Destroy(gameObject);
        }

        public void OnBuy()
        {
            buyButton.interactable      = false;
            buyMaxButton.interactable   = false;

            callback(1, UpdatePanel);
        }

        public void OnBuyMax()
        {
            buyButton.interactable      = false;
            buyMaxButton.interactable   = false;

            callback(BuyMaxAmount, UpdatePanel);
        }
    }
}