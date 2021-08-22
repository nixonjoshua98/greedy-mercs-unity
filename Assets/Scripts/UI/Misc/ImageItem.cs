using UnityEngine;
using UnityEngine.UI;

namespace GM.UI
{
    using GM.Data;

    public class ImageItem : MonoBehaviour
    {
        public ItemType Item;

        void Awake()
        {
            SetSprite();
        }


        public void Set(ItemType item)
        {
            Item = item;

            SetSprite();
        }


        void SetSprite()
        {
            GetComponent<Image>().sprite = GameData.Get.Items.Get(Item).Icon;
        }
    }
}