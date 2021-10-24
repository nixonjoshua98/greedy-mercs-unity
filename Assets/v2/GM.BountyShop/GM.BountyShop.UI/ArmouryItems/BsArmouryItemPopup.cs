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
        public TMP_Text TierText;
        public TMP_Text LevelText;
        public TMP_Text OwnedText;
        public TMP_Text PurchaseCostText;
        [Space]
        public Image IconImage;

        Action purchaseCallback; // Can be null

        public void Assign(Data.BountyShopArmouryItem item, Action _purchaseCallback)
        {
            purchaseCallback = _purchaseCallback;

            Assign(item);

            UpdateUI();
        }

        protected override void OnAssignedItem()
        {
            PurchaseCostText.text = AssignedItem.PurchaseCost.ToString();
            NameText.text = AssignedItem.ItemName;
            IconImage.sprite = AssignedItem.Icon;           
            TierText.color = AssignedItem.Item.Config.Colour;
            TierText.text = AssignedItem.Item.Config.DisplayText;
        }

        void UpdateUI()
        {
            if (App.Data.Armoury.TryGetOwnedItem(AssignedItem.ArmouryItem, out Armoury.Data.ArmouryItemData result))
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

        // == Callbacks == //
        public void OnPurchaseButton()
        {
            App.Data.BountyShop.PurchaseItem(AssignedItem.Id, (success) =>
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
