using UnityEngine;
using UnityEngine.UI;

namespace GM.UI
{
    using GM.Data;

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
            GetComponent<Image>().sprite = App.Data.GameItems[item].Icon;
        }
    }
}