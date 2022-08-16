using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SRC.Armoury.UI
{
    public class ArmouryItemPopup : ArmouryItemUIObject
    {
        public TMP_Text NameText;
        public TMP_Text LevelOwnedText;
        public TMP_Text DamageText;
        public TMP_Text UpgradeText;
        [Space]
        public Image IconImage;

        protected override void OnAssigned()
        {
            SetStaticUIElements();
        }

        private void SetStaticUIElements()
        {
            IconImage.sprite = AssignedItem.Icon;
            NameText.text = AssignedItem.Name;
        }

        private void FixedUpdate()
        {
            LevelOwnedText.text = $"Lvl. <color=orange>{AssignedItem.CurrentLevel}</color> | Owned <color=white>{AssignedItem.NumOwned}</color>";
            DamageText.text = GetBonusText();
            UpgradeText.text = Format.Number(AssignedItem.UpgradeCost());
        }

        public void OnUpgradeButton()
        {
            App.Armoury.UpgradeItem(AssignedItem.ID, (success, resp) =>
            {
                if (success)
                {
                    App.Inventory.ArmouryPointsChanged.Invoke(resp.UpgradeCost * -1);
                }
            });
        }
    }
}