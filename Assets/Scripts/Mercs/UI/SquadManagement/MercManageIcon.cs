using UnityEngine;
using UnityEngine.UI;

namespace GM.Mercs.UI
{
    public class MercManageIcon : MonoBehaviour
    {
        public Image IconImage;

        public void SetIcon(Sprite spr)
        {
            IconImage.sprite = spr;
        }
    }
}