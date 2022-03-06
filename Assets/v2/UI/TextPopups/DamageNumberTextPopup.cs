using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
