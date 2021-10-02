using UnityEngine;
using UnityEngine.UI;

namespace GM.UI
{
    public class ItemTextPopup : TextPopup
    {
        [SerializeField] Image itemImage;

        Color originalImageColor;

        public void Setup(GM.Items.Data.ItemType item, string val)
        {
            Setup(val);

            originalImageColor = itemImage.color;

            GetComponentInChildren<ImageItem>()?.Set(item);
        }

        protected override float ProcessFade()
        {
            float percent = base.ProcessFade();

            itemImage.color = new Color(itemImage.color.r, itemImage.color.g, itemImage.color.b, originalImageColor.a * percent);

            return percent;
        }
    }
}
