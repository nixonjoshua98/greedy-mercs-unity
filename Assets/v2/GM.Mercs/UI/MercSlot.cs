using TMPro;
using UnityEngine;
using UnityEngine.UI;
using MercID = GM.Common.Enums.MercID;

namespace GM.Mercs.UI
{
    public class MercSlot : MercUIObject
    {
        [Header("Prefabs")]
        public GameObject PopupObject;

        [Header("References")]
        public Image IconImage;
        public TMP_Text NameText;
        public TMP_Text LevelText;
        public TMP_Text DamageText;
        [Space]
        public GM.UI.VStackedButton UpgradeButton;

        int _buyAmount;
        protected int buyAmount
        {
            get
            {
                return Mathf.Min(_buyAmount, Common.Constants.MAX_MERC_LEVEL - AssignedMerc.CurrentLevel);
            }
        }

        public void Assign(MercID merc, GM.UI.AmountSelector selector)
        {
            Assign(merc); 

            _buyAmount = selector.Current;

            selector.E_OnChange.AddListener((val) => { _buyAmount = val; });
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
            BigDouble upgradeCost = App.Cache.MercUpgradeCost(AssignedMerc, buyAmount);

            LevelText.text = FormatLevel(AssignedMerc.CurrentLevel);
            DamageText.text = GetBonusText();

            UpgradeButton.SetText("MAX LEVEL", "");

            if (!AssignedMerc.IsMaxLevel)
            {
                UpgradeButton.SetText($"x{buyAmount}", Format.Number(upgradeCost));
            }

            UpgradeButton.interactable = !AssignedMerc.IsMaxLevel && App.Data.Inv.Gold >= upgradeCost;
        }

        string GetBonusText() => $"<color=orange>{Format.Number(AssignedMerc.DamagePerAttack)}</color> DMG";

        // == Callbacks == //
        public void OnUpgradeButton()
        {
            BigDouble upgradeCost = App.Cache.MercUpgradeCost(AssignedMerc, buyAmount);

            bool willExceedMaxLevel = AssignedMerc.CurrentLevel + buyAmount > Common.Constants.MAX_MERC_LEVEL;
            bool canAffordUpgrade = App.Data.Inv.Gold >= upgradeCost;

            if (!willExceedMaxLevel && canAffordUpgrade)
            {
                AssignedMerc.IncrementLevel(buyAmount);

                App.Data.Inv.Gold -= upgradeCost;
            }
        }

        /// <summary>
        /// Callback from UI to open the merc popup
        /// </summary>
        public void OnInfoButton()
        {
            InstantiateUI<MercPopup>(PopupObject).Assign(AssignedMerc.Id);
        }
    }
}