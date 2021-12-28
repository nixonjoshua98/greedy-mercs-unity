using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM._Old
{
    public class Stage1000 : Core.GMMonoBehaviour
    {
        public void OnClick()
        {
            App.Data.GameState.Stage += 100;
        }

        public void AddGold()
        {
            App.Data.Inv.Gold += BigDouble.Parse("1e500");
        }
    }
}
