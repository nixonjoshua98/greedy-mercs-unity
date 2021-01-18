using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace Armoury.UI
{
    public class WeaponFrame : MonoBehaviour
    {
        [SerializeField] Image Icon;

        public void Init(Sprite spr)
        {
            Utils.UI.SetImageScaleW(Icon, spr, Icon.preferredWidth);
        }
    }
}