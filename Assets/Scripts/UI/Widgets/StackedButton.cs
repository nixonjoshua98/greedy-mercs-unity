using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GM.UI
{
    public class StackedButton : MonoBehaviour
    {
        public Text TextOne;
        public Text TextTwo;

        [SerializeField] Button button;

        public void SetText(string s1, string s2)
        {
            TextOne.text = s1;
            TextTwo.text = s2;
        }


        public void Toggle(bool val)
        {
            button.interactable = val;
        }
    }
}