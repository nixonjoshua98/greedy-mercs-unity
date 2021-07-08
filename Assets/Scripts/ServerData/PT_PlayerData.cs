using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM
{
    public class PT_PlayerData
    { 
        static PT_PlayerData Instance = null;

        public static void CreateInstance()
        {
            Instance = new PT_PlayerData();
        }

        public static PT_PlayerData Get()
        {
            return Instance;
        }
    }
}
