using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnitID = GM.Common.Enums.UnitID;

namespace GM.Mercs.UI
{
    public class SquadMercSlot : MercUIObject
    {
        [Header("Prefabs")]
        public GameObject PopupObject;

        [Header("References")]
        public Image IconImage;
        public TMP_Text NameText;
        public TMP_Text LevelText;
        public TMP_Text DamageText;

        [SerializeField] TMP_Text EnergyPercentageText;
        [SerializeField] Slider EnergySlider;
        [SerializeField] Slider ExcessEnergySlider;
        [Space]
        public GM.UI.VStackedButton UpgradeButton;

        int _buyAmount;
        protected int BuyAmount => MathUtils.NextMultipleMax(AssignedMerc.CurrentLevel, _buyAmount, Common.Constants.MAX_MERC_LEVEL);

        public void Assign(UnitID merc, GM.UI.AmountSelector selector)
        {
            _buyAmount = selector.Current;

            Assign(merc); 

            selector.E_OnChange.AddListener((val) => _buyAmount = val);
        }

        protected override void OnAssigned()
        {
            IconImage.sprite = AssignedMerc.Icon;
            NameText.text = AssignedMerc.Name;

            UpdateUI();
        }

        void FixedUpdate()
        {
            UpdateUI();
        }

        void UpdateUI()
        {
            // Energy
            EnergyPercentageText.text   = Format.Percentage(AssignedMerc.CurrentSpawnEnergyPercentage, 0);
            EnergySlider.value          = Mathf.Clamp(AssignedMerc.CurrentSpawnEnergyPercentage, 0, 1);
            ExcessEnergySlider.value    = Mathf.Clamp(AssignedMerc.CurrentSpawnEnergyPercentage - 1, 0, 1);

            LevelText.text = FormatLevel(AssignedMerc.CurrentLevel);
            DamageText.text = GetBonusText();

            UpgradeButton.SetText("MAX LEVEL", "");

            if (!AssignedMerc.IsMaxLevel)
            {
                UpgradeButton.SetText($"x{BuyAmount}", Format.Number(AssignedMerc.UpgradeCost(BuyAmount)));
            }

            UpgradeButton.interactable = !AssignedMerc.IsMaxLevel && App.GMData.Inv.Gold >= AssignedMerc.UpgradeCost(BuyAmount);
        }

        string GetBonusText() => $"<color=orange>{Format.Number(AssignedMerc.DamagePerAttack)}</color> DMG";


        public void OnUpgradeButton()
        {
            BigDouble upgradeCost = App.GMCache.MercUpgradeCost(AssignedMerc, BuyAmount);

            bool willExceedMaxLevel = AssignedMerc.CurrentLevel + BuyAmount > Common.Constants.MAX_MERC_LEVEL;
            bool canAffordUpgrade = App.GMData.Inv.Gold >= upgradeCost;

            if (!willExceedMaxLevel && canAffordUpgrade)
            {
                AssignedMerc.CurrentLevel += BuyAmount;

                App.GMData.Inv.Gold -= upgradeCost;

                App.Events.GoldChanged.Invoke(upgradeCost * -1);
            }
        }

        /// <summary> Callback from UI to open the merc popup </summary>
        public void OnInfoButton()
        {
            InstantiateUI<MercPopup>(PopupObject).Assign(AssignedMerc.ID);
        }
    }
}