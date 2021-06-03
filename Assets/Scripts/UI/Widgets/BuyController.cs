using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM.UI
{
    public class BuyController : ToggleController
    {
        public int[] BuyAmounts;

        protected override void InvokeEvent(int index)
        {
            OnValueChanged.Invoke(BuyAmounts[index]);
        }
    }
}