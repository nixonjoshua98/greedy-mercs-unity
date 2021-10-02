using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM._Debug
{
    public class Stage1000 : Core.GMMonoBehaviour
    {
        public void OnClick()
        {
            GameManager.Instance.state.Stage += 100;
        }

        public void AddGold()
        {
            App.Data.Inv.Gold += BigDouble.Parse("1e500");
        }
    }
}
