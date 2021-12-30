using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM.UI.Popups
{
    public class MercSummaryPopupButton : Core.GMMonoBehaviour
    {
        public GameObject PopupObject;

        public void OnClick()
        {
            InstantiateUI(PopupObject);
        }
    }
}
