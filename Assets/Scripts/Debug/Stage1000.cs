using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM._Debug
{
    public class Stage1000 : MonoBehaviour
    {
        public void OnClick()
        {
            GameState.Stage.stage += 1000;
        }

        public void AddGold()
        {
            GameState.Player.gold += BigDouble.Parse("1e5000");
        }
    }
}
