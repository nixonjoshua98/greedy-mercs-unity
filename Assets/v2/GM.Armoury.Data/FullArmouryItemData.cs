using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Armoury.Data
{
    public struct FullArmouryItemData
    {
        public GM.Data.ArmouryItemData Game;

        public FullArmouryItemData(GM.Data.ArmouryItemData game)
        {
            Game = game;
        }
    }
}
