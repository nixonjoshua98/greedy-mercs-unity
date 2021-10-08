
using UnityEngine;
using UnityEngine.UI;

namespace GM.Armoury.UI
{
    using GM.UI;

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
        Data.ArmouryItemData ItemData => App.Data.Armoury.GetItem(ArmouryItemID);

        public void Init(int itemId)
        {
            ArmouryItemID = itemId;

            SetStaticInterface();
            UpdateUI();
        }


        void SetStaticInterface()
        {
            nameText.text = ItemData.Game.Name.ToUpper();

            colouredWeapon.sprite = shadowWeapon.sprite = ItemData.Game.Icon;

            stars.Show(ItemData.Game.Tier + 1);
        }


        void UpdateUI()
        {
            int levelCost = ItemData.UpgradeCost(); // Load the value to avoid re-calculation

            long armouryPoints = App.Data.Inv.ArmouryPoints;

            // Formatting
            double currentDamage = ItemData.WeaponDamage;
            string currentDmgString = FormatString.Number(currentDamage * 100, prefix: "%");

            // Text 
            damageText.text = $"<color=white>Mercenary Damage:</color> {currentDmgString}";
            levelCostText.text = levelCost.ToString();

            // Update the evolve level slider
            evolveSlider.maxValue = ItemData.Game.EvoLevelCost;
            evolveSlider.value = (ItemData.User.NumOwned - 1);

            // Buttons
            evolveButton.interactable = ItemData.ReadyToEvolve;
            levelButton.interactable = armouryPoints >= levelCost;
        }


        // = = = Button Callbacks = = = //

        public void OnEvolveButton()
        {
            App.Data.Armoury.EvolveItem(ArmouryItemID, (success) => { UpdateUI(); });
        }


        public void OnUpgradeButton()
        {
            App.Data.Armoury.UpgradeItem(ArmouryItemID, (success) => { UpdateUI(); });
        }

        // = = = ^
    }
}
