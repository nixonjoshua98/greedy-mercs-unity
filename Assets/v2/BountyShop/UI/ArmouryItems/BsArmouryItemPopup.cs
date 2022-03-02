using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GM.BountyShop.UI
{
    public class BsArmouryItemPopup : BSArmouryItemObject
    {
        [Header("References")]
        public TMP_Text NameText;
        public TMP_Text LevelText;
        public TMP_Text OwnedText;
        [Space]
        public Image IconImage;

        Action purchaseCallback; // Can be null

        public void Assign(Data.BountyShopArmouryItem item, Action _purchaseCallback)
        {
            Assign(item);

            purchaseCallback = _purchaseCallback;
        }

        protected override void OnAssignedItem()
        {
            NameText.text = AssignedItem.ItemName;
            IconImage.sprite = AssignedItem.Icon;

            UpdateUI();
        }

        void UpdateUI()
        {
            if (App.GMData.Armoury.TryGetOwnedItem(AssignedItem.ArmouryItemId, out Armoury.Data.ArmouryItemData result))
            {
                OwnedText.text = $"Owned <color=orange>{result.NumOwned}</color>";
                LevelText.text = $"Level <color=orange>{result.CurrentLevel}</color>";
            }
            else
            {
                OwnedText.text = "Owned <color=orange>0</color>";
                LevelText.text = "Level <color=orange>1</color>";
            }
        }

        public void OnPurchaseButton()
        {
            App.GMData.BountyShop.PurchaseArmouryItem(AssignedItem.Id, (success) =>
            {
                UpdateUI();

                purchaseCallback?.Invoke();

                if (!AssignedItem.InStock)
                {
                    Destroy(gameObject);
                }
            });
        }
    }
}
