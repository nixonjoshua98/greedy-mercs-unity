using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GM.Mercs.UI
{
    public class MercSlot : MercUIObject
    {
        [Header("References")]
        public Image IconImage;
        public TMP_Text NameText;
        public TMP_Text LevelText;

        protected override void OnAssigned()
        {
            IconImage.sprite = AssignedMerc.Icon;
            NameText.text = AssignedMerc.Name;
        }

        void FixedUpdate()
        {
            LevelText.text = $"Level {AssignedMerc.CurrentLevel}";
        }
    }
}