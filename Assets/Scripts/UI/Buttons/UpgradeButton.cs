using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GreedyMercs.UI
{
    public class UpgradeButton : MonoBehaviour
    {
        public Text topText;
        public Text btmText;

        [Space]

        public Image topImage;
        public Image btmImage;

        public void Set(string t1, string t2)
        {
            topText.text = t1;
            btmText.text = t2;
        }

        public void Set(Color col)
        {
            topImage.color = btmImage.color = col;
        }
    }
}