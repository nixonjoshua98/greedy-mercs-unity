using UnityEngine;

namespace GM.UI
{
    public class DamageNumberTextPopup : TextPopup
    {
        public void Set(string value, Color col)
        {
            Reset();

            Text.text = value;
            Text.color = col;
        }
    }
}
