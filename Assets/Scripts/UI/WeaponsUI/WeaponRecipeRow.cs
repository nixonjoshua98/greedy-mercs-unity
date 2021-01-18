using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace UI.Weapons
{
    public class WeaponRecipeRow : MonoBehaviour
    {
        [SerializeField] Image Icon;

        [SerializeField] Text Text;

        public void Init(Sprite icon, string text)
        {
            Icon.sprite = icon;

            Text.text = text;
        }
    }
}