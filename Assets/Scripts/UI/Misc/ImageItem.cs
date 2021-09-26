using UnityEngine;
using UnityEngine.UI;
using GM.Core;

namespace GM.UI
{
    public class ImageItem : Core.GMMonoBehaviour
    {
        public GM.Items.Data.ItemType Item;

        void Awake()
        {
            SetSprite(Item);
        }


        public void Set(GM.Items.Data.ItemType item)
        {
            SetSprite(item);
        }


        void SetSprite(GM.Items.Data.ItemType item)
        {
            GetComponent<Image>().sprite = App.Data.Items[item].Icon;
        }
    }
}