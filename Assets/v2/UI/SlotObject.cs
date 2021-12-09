using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM.UI
{
    public abstract class SlotObject : Core.GMMonoBehaviour
    {
        public string FormatLevel(int level)
        {
            return $"Lvl. <color=orange>{level}</color>";
        }
    }
}