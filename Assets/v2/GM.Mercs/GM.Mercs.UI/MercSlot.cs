using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GM.Mercs.UI
{
    public class MercSlot : MercUIObject
    {
        [Header("References")]
        public Image IconImage;
        public TMP_Text NameText;
        public TMP_Text LevelText;
        public TMP_Text DamageText;
        [Space]
        public GM.UI.VStackedButton Button;

        int _buyAmount;
        protected int buyAmount
        {
            get
            {
                if (_buyAmount == -1)
                    return Formulas.AffordCharacterLevels(AssignedMerc.Id);

                return Mathf.Min(_buyAmount, GM.Constants.MAX_MERC_LEVEL - AssignedMerc.CurrentLevel);
            }
        }

        public void Assign(MercID merc, GM.UI.AmountSelector selector)
        {
            Assign(merc);

            _buyAmount = selector.Value;

            selector.E_OnChange.AddListener((val) => { _buyAmount = val; });
        }

        protected override void OnAssigned()
        {
            IconImage.sprite = AssignedMerc.Icon;
            NameText.text = AssignedMerc.Name;
        }

        void FixedUpdate()
        {
            LevelText.text = $"Level {AssignedMerc.CurrentLevel}";
            DamageText.text = $"{Format.Number(StatsCache.TotalMercDamage(AssignedMerc.Id))}";

            Button.SetText("Max Level", "");

            if (!AssignedMerc.IsMaxLevel)
            {
                Button.SetText($"x{buyAmount}", Format.Number(Formulas.MercLevelUpCost(AssignedMerc.CurrentLevel, buyAmount, AssignedMerc.UnlockCost)));
            }
        }

        // == Callbacks == //
        public void OnUpgrade()
        {
            BigDouble upgradeCost = App.Cache.MercUpgradeCost(AssignedMerc, buyAmount);

            bool willExceedMaxLevel = AssignedMerc.CurrentLevel + buyAmount > Constants.MAX_MERC_LEVEL;
            bool canAffordUpgrade = App.Data.Inv.Gold >= upgradeCost;

            if (!willExceedMaxLevel && canAffordUpgrade)
            {
                AssignedMerc.LevelUp(buyAmount);

                App.Data.Inv.Gold -= upgradeCost;

                GM.Events.GlobalEvents.E_OnMercLevelUp.Invoke(AssignedMerc.Id);
            }
        }
    }
}