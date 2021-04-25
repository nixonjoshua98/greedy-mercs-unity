﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GM.Interface
{
    public class DefaultUpgradeButton : MonoBehaviour
    {
        public Text TopText;
        public Text BottomText;

        public void SetText(string top, string btm)
        {
            TopText.text = top;
            BottomText.text = btm;
        }
    }
}