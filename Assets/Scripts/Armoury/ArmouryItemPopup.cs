using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GM.Armoury.UI
{
    using GM.UI;
    using GM.Data;

    public class ArmouryItemPopup : MonoBehaviour
    {
        [SerializeField] Text nameText;
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

        int _itemId;


        public void Init(int itemId)
        {
            _itemId = itemId;

            ArmouryItemData data = GameData.Get().Armoury.Get(_itemId);

            nameText.text = data.Name.ToUpper();

            colouredWeapon.sprite = shadowWeapon.sprite = data.Icon;

            stars.Show(data.Tier);

            UpdateUI();
        }


        void UpdateUI()
        {
            int maxEvoLevel     = GameData.Get().Armoury.MaxEvolveLevel;
            int evoLevelCost    = GameData.Get().Armoury.EvoLevelCost;
            int armouryPoints   = UserData.Get().Inventory.IronIngots;
            int levelCost       = GameData.Get().Armoury.LevelCost(_itemId);

            // Grab the current state
            ArmouryItemState state = UserData.Get().Armoury.Get(_itemId);

            // Formatting
            double currentDamage    = UserData.Get().Armoury.WeaponDamage(_itemId);
            string currentDmgString = FormatString.Number(currentDamage * 100, prefix: "%");

            // Text 
            damageText.text     = $"<color=white>Mercenary Damage:</color> {currentDmgString}";
            levelCostText.text  = levelCost.ToString();

            // Update the evolve level slider
            evolveSlider.maxValue   = evoLevelCost;
            evolveSlider.value      = (state.owned - 1);

            // Buttons
            evolveButton.interactable   = state.owned >= (evoLevelCost + 1) && state.evoLevel < maxEvoLevel;
            levelButton.interactable    = armouryPoints >= GameData.Get().Armoury.LevelCost(_itemId);
        }


        // = = = Button Callbacks = = = //

        public void OnEvolveButton()
        {
            UserData.Get().Armoury.EvolveItem(_itemId, () => { UpdateUI(); });
        }


        public void OnUpgradeButton()
        {
            UserData.Get().Armoury.UpgradeItem(_itemId, () => { UpdateUI(); });
        }


        public void OnClosePopup() 
        { 
            Destroy(gameObject); 
        }

        // = = = ^
    }
}
