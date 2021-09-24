
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
        GM.Armoury.Data.FullArmouryItemData ItemData => App.Data.Armoury[ArmouryItemID];

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
            int evoLevelCost = ItemData.Game.EvoLevelCost;
            int levelCost = ItemData.UpgradeCost();

            long armouryPoints = App.UserData.Inventory.IronIngots;
           
            // Formatting
            double currentDamage = App.UserData.Armoury.WeaponDamage(ArmouryItemID);
            string currentDmgString = FormatString.Number(currentDamage * 100, prefix: "%");

            // Text 
            damageText.text = $"<color=white>Mercenary Damage:</color> {currentDmgString}";
            levelCostText.text = levelCost.ToString();

            // Update the evolve level slider
            evolveSlider.maxValue = evoLevelCost;
            evolveSlider.value = (ItemData.User.NumOwned - 1);

            // Buttons
            evolveButton.interactable = ItemData.ReadyToEvolve;
            levelButton.interactable = armouryPoints >= levelCost;
        }


        // = = = Button Callbacks = = = //

        public void OnEvolveButton()
        {
            App.UserData.Armoury.EvolveItem(ArmouryItemID, () => { UpdateUI(); });
        }


        public void OnUpgradeButton()
        {
            App.UserData.Armoury.UpgradeItem(ArmouryItemID, () => { UpdateUI(); });
        }

        // = = = ^
    }
}
