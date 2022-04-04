using GM.Common.Enums;
using UnityEngine.UI;

namespace GM.UI
{
    public class ImageItem : Core.GMMonoBehaviour
    {
        public CurrencyType Item;

        private void Awake()
        {
            SetSprite(Item);
        }

        public void Set(CurrencyType item)
        {
            SetSprite(item);
        }

        private void SetSprite(CurrencyType item)
        {
            GetComponent<Image>().sprite = App.Items.GetItem(item).Icon;
        }
    }
}