using GM.BountyShop.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GM.BountyShop.UI
{
    public class BountyShopArmouryItemSlot : GM.Core.GMMonoBehaviour
    {
        [Header("Components")]
        [SerializeField] Image ItemImage;
        [SerializeField] Image BackgroundImage;
        [Space]
        [SerializeField] TMP_Text CostText;

        public void Set(BountyShopArmouryItem item)
        {
            CostText.text = item.PurchaseCost.ToString();

            ItemImage.sprite = item.Item.Icon;
            BackgroundImage.sprite = item.Item.GradeConfig.BackgroundSprite;
        }

        public void Button_OnClick()
        {

        }
    }
}
