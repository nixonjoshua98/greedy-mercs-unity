using GM.BountyShop.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GM.BountyShop.UI
{
    public class BountyShopArmouryItemSlot : GM.Core.GMMonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] GameObject PurchaseConfirmObject;

        [Header("Components")]
        [SerializeField] Image ItemImage;
        [SerializeField] Image BackgroundImage;
        [Space]
        [SerializeField] TMP_Text CostText;
        [Space]
        [SerializeField] GameObject SoldOverlay;

        BountyShopArmouryItem Item;

        public void Set(BountyShopArmouryItem item)
        {
            Item = item;

            CostText.text = item.PurchaseCost.ToString();

            ItemImage.sprite = item.Item.Icon;
            BackgroundImage.sprite = item.Item.GradeConfig.BackgroundSprite;

            SoldOverlay.SetActive(!item.InStock);
        }
        
        void PurchaseItem()
        {
            App.BountyShop.PurchaseArmouryItem(Item.ID, (success, resp) =>
            {


            });
        }

        public void Button_OnClick()
        {

        }
    }
}
