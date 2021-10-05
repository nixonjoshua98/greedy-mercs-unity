using UnityEngine;
using UnityEngine.UI;
using GM.Core;

namespace GM.UI
{
    public class ImageItem : Core.GMMonoBehaviour
    {
        public GM.CurrencyItems.Data.CurrencyType Item;

        void Awake()
        {
            SetSprite(Item);
        }


        public void Set(GM.CurrencyItems.Data.CurrencyType item)
        {
            SetSprite(item);
        }


        void SetSprite(GM.CurrencyItems.Data.CurrencyType item)
        {
            GetComponent<Image>().sprite = App.Data.Items.GetItem(item).Icon;
        }
    }
}