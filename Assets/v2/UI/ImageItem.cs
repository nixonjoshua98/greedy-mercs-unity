using GM.Common.Enums;
using UnityEngine.UI;

namespace GM.UI
{
    public class ImageItem : Core.GMMonoBehaviour
    {
        public CurrencyType Item;

        void Awake()
        {
            SetSprite(Item);
        }
        
        public void Set(CurrencyType item)
        {
            SetSprite(item);
        }

        void SetSprite(CurrencyType item)
        {
            GetComponent<Image>().sprite = App.GMData.Items.GetItem(item).Icon;
        }
    }
}