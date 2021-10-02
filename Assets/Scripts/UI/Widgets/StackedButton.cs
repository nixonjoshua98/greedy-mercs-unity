
using UnityEngine;
using UnityEngine.UI;

namespace GM.UI
{
    public class StackedButton : MonoBehaviour
    {
        public Text TextOne;
        public Text TextTwo;

        public void SetText(string s1, string s2)
        {
            TextOne.text = s1;
            TextTwo.text = s2;
        }
    }
}