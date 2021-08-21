using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GM.UI
{
    using GM.Data;

    public class ItemTextPopup : TextPopup
    {
        [SerializeField] Image itemImage;

        Color originalImageColor;

        public void Setup(ItemType item, string val, Color col)
        {
            Setup(val, col);

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
