using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GM.Armoury.UI
{
    using GM.UI;
    using GM.Data;

    public class ArmouryItemPopup : Core.GMMonoBehaviour
    {
        [SerializeField] TMPro.TMP_Text nameText;
        [SerializeField] Text damageText;
        [SerializeField] Text levelCostText;
        [Space]
        [SerializeField] Slider evolveSlider;
        [Space]
        [SerializeField] Button evolveButton;
        [SerializeField] Button levelButton;

        [Header("Weapon")]
        [SerializeField] Image colouredWeapon;
        [SerializeField] Image shadowWeapon;

        [Header("References")]
        [SerializeField] StarRatingController stars;

        // ...
        int ArmouryItemID;
        ArmouryItemData ItemData => App.GameData.Armoury.Get(ArmouryItemID);

        public void Init(int itemId)
        {
            ArmouryItemID = itemId;

            SetStaticInterface();
            UpdateUI();
        }


        void SetStaticInterface()
        {
            nameText.text = ItemData.Name.ToUpper();

            colouredWeapon.sprite = shadowWeapon.sprite = ItemData.Icon;

            stars.Show(ItemData.Tier + 1);
        }


        void UpdateUI()
        {
            // Shorthand to armoury data
            GameArmouryData armouryData = App.GameData.Armoury;
            UserArmoury userData = App.PlayerData.Armoury;

            int evoLevelCost    = armouryData.EvoLevelCost;
            int levelCost       = armouryData.LevelCost(ArmouryItemID);

            long armouryPoints = App.PlayerData.Inventory.IronIngots;

            // Grab the current state
            ArmouryItemState state = App.PlayerData.Armoury.Get(ArmouryItemID);

            // Formatting
            double currentDamage    = userData.WeaponDamage(ArmouryItemID);
            string currentDmgString = FormatString.Number(currentDamage * 100, prefix: "%");

            // Text 
            damageText.text     = $"<color=white>Mercenary Damage:</color> {currentDmgString}";
            levelCostText.text  = levelCost.ToString();

            // Update the evolve level slider
            evolveSlider.maxValue   = evoLevelCost;
            evolveSlider.value      = (state.owned - 1);

            // Buttons
            evolveButton.interactable = userData.CanEvolveItem(ArmouryItemID);
            levelButton.interactable = armouryPoints >= levelCost;
        }


        // = = = Button Callbacks = = = //

        public void OnEvolveButton()
        {
            App.PlayerData.Armoury.EvolveItem(ArmouryItemID, () => { UpdateUI(); });
        }


        public void OnUpgradeButton()
        {
            App.PlayerData.Armoury.UpgradeItem(ArmouryItemID, () => { UpdateUI(); });
        }

        // = = = ^
    }
}
