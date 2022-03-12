﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM._Old
{
    public class Stage1000 : Core.GMMonoBehaviour
    {
        public void OnClick()
        {
            App.GameState.Stage += 100;
        }

        public void AddGold()
        {
            App.Inventory.Gold += BigDouble.Parse("1e500");
        }
    }
}
