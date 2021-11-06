using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace GM.Mercs.UI
{
    public class MercPopup : MercUIObject
    {
        [Header("Prefabs")]
        public GameObject TextObject;

        [Header("References")]
        public TMP_Text NameText;
        public Transform BonusTextParent;

        void FixedUpdate()
        {
            NameText.text = $"{AssignedMerc.Name} Lvl. <color=orange>{AssignedMerc.CurrentLevel}</color>";
        }
    }
}
