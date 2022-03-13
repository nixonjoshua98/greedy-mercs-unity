using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;

namespace GM.UI
{
    public class PillBadge : MonoBehaviour
    {
        [SerializeField] GameObject InnerObject;
        [SerializeField] TMP_Text Text;

        public void Show(string txt = "!") => _Show(txt);
        public void Show(int value) => _Show(value > 9 ? "9+" : value.ToString());


        void _Show(string txt)
        {
            Text.text = txt;
            InnerObject.SetActive(true);
        }

        public void Hide()
        {
            InnerObject.SetActive(false);
        }
    }
}
