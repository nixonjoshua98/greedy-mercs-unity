using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

namespace GM.Armoury.Data
{
    public class ArmouryData
    {
        public GameArmouryDictionary Game;

        public ArmouryData(JSONNode userJSON, JSONNode gameJSON)
        {
            Game = new GameArmouryDictionary(gameJSON);
        }


        public FullArmouryItemData this[int key]
        {
            get => new FullArmouryItemData(Game[key], UserData.Get.Armoury.Get(key));
        }
    }
}
