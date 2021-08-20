using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM.UI
{
    using GM.Data;

    public class ItemTextPopup : TextPopup
    {
        public void Setup(ItemType item, string val, Color col)
        {
            Setup(val, col);

            GetComponentInChildren<ImageItem>()?.Set(item);
        }
    }
}
