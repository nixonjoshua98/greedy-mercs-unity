using System;
using System.Numerics;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Vector3 = UnityEngine.Vector3;

namespace WeaponsUI
{
    using CharacterData;
    using WeaponData;

    public class WeaponInfoPanel : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] GameObject weaponRecipeParent;

        [Header("Prefabs")]
        [SerializeField] GameObject WeaponRecipeRowObject;

        [Header("Sprites")]
        [SerializeField] Sprite TransparentSquare;

        [Header("Text")]
        [SerializeField] Text titleText;
        [SerializeField] Text descText;
        [SerializeField] Text buyMaxText;
        [SerializeField] Text weaponDamageText;

        [Header("Images")]
        [SerializeField] Image weaponImage;
        [SerializeField] Image characterImage;

        [Space]
        [SerializeField] Button buyButton;
        [SerializeField] Button buyMaxButton;

        Action<int, Action> callback;

        WeaponStaticData weaponStaticData {  get { return StaticData.Weapons.GetWeaponAtIndex(weaponIndex); } }

        // - Scriptables
        CharacterSO character;
        ScriptableWeapon weapon;

        int weaponIndex;

        int BuyMaxAmount
        {
            get
            {
                int weaponsOwned    = GameState.Weapons.Get(character.CharacterID, weaponIndex);
                int weaponsLeft     = weaponStaticData.maxOwned - weaponsOwned;

                Dictionary<int, int> recipe = weaponStaticData.mergeRecipe;

                int maxMergeBuy = int.MaxValue;

                foreach (KeyValuePair<int, int> entry in recipe)
                {
                    int prevWeaponOwned = GameState.Weapons.Get(character.CharacterID, entry.Key);

                    maxMergeBuy = Mathf.Min(maxMergeBuy, prevWeaponOwned / entry.Value);
                }

                int maxBuy = (int)BigInteger.Min(maxMergeBuy, weaponStaticData.buyCost > 0 ? GameState.Player.bountyPoints / weaponStaticData.buyCost : weaponStaticData.maxOwned);

                return Mathf.Min(maxBuy, weaponsLeft);
            }
        }

        public void Init(CharacterSO chara, ScriptableWeapon _weapon, int _weaponIndex, Action<int, Action> func)
        {
            callback    = func;
            character   = chara;
            weapon      = _weapon;
            weaponIndex = _weaponIndex;

            CreateRecipePanel(weaponStaticData.mergeRecipe);

            titleText.text  = chara.name;
            descText.text   = string.Format("Tier {0} Weapon", weaponIndex + 1);

            weaponImage.sprite      = weapon.icon;
            characterImage.sprite   = chara.icon;

            UpdatePanel();
        }

        void CreateRecipePanel(Dictionary<int, int> recipe)
        {
            if (weaponStaticData.buyCost > 0)
            {
                ScriptableWeapon weapon = character.weapons[weaponIndex];

                GameObject o = Utils.UI.Instantiate(WeaponRecipeRowObject, weaponRecipeParent.transform, Vector3.zero);

                WeaponRecipeRow row = o.GetComponent<WeaponRecipeRow>();

                row.Init(weapon.icon, string.Format("{0}x Bounty Points", weaponStaticData.buyCost));
            }


            foreach (KeyValuePair<int, int> entry in recipe)
            {
                ScriptableWeapon recipeWeapon = character.weapons[entry.Key];

                GameObject o = Utils.UI.Instantiate(WeaponRecipeRowObject, weaponRecipeParent.transform, Vector3.zero);

                WeaponRecipeRow row = o.GetComponent<WeaponRecipeRow>();

                row.Init(recipeWeapon.icon, string.Format("{0}x Tier {1} Weapons", entry.Value, entry.Key + 1));
            }
        }

        void UpdatePanel()
        {
            int weaponsOwned = GameState.Weapons.Get(character.CharacterID, weaponIndex);

            weaponDamageText.text   = Utils.Format.FormatNumber(Formulas.CalcWeaponDamageMultiplier(weaponIndex, weaponsOwned) * 100) + "%";
            buyMaxText.text         = "Buy x" + BuyMaxAmount;

            if (weaponStaticData.buyCost > 0)
                UpdateBuyWeapon();

            else
                UpdateMergeWeapon();
        }

        void UpdateBuyWeapon()
        {
            buyButton.interactable      = BuyMaxAmount > 0 && GameState.Player.bountyPoints >= (weaponStaticData.buyCost * 1);
            buyMaxButton.interactable   = BuyMaxAmount > 0 && GameState.Player.bountyPoints >= (weaponStaticData.buyCost * BuyMaxAmount);
        }

        void UpdateMergeWeapon()
        {
            int prevWeaponOwned = GameState.Weapons.Get(character.CharacterID, weaponIndex - 1);

            buyButton.interactable      = BuyMaxAmount > 0 && prevWeaponOwned >= (weaponStaticData.mergeCost * 1);
            buyMaxButton.interactable   = BuyMaxAmount > 0 && prevWeaponOwned >= (weaponStaticData.mergeCost * BuyMaxAmount);
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