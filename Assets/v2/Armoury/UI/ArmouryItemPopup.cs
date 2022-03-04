using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GM.Armoury.UI
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

        void SetStaticUIElements()
        {
            IconImage.sprite = AssignedItem.Icon;
            NameText.text = AssignedItem.ItemName;
        }

        void FixedUpdate()
        {
            LevelOwnedText.text = $"{FormatLevel(AssignedItem.CurrentLevel)} | Owned <color=white>{AssignedItem.NumOwned}</color>";
            DamageText.text = GetBonusText();
            UpgradeText.text = Format.Number(AssignedItem.UpgradeCost());
        }

        public void OnUpgradeButton()
        {
            App.GMData.Armoury.UpgradeItem(AssignedItem.Id, (success, resp) =>
            {
                if (success)
                {
                    App.Events.ArmouryPointsChanged.Invoke(resp.UpgradeCost * -1);
                }
            });
        }
    }
}