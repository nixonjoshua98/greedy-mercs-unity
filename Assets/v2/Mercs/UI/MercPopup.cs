using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GM.Mercs.UI
{
    public class MercPopup : MercUIObject
    {
        [Header("Prefabs")]
        public GameObject PassiveSlotObject;

        [Header("References")]
        public TMP_Text NameText;
        public Image IconImage;
        public Transform PassivesParent;

        protected override void OnAssigned()
        {
            IconImage.sprite = AssignedMerc.Icon;

            foreach (MercPassiveReference p in AssignedMerc.Passives)
            {
                Instantiate<MercPassiveSlot>(PassiveSlotObject, PassivesParent).Assign(p, AssignedMerc.IsPassiveUnlocked(p));
            }
        }

        void FixedUpdate()
        {
            NameText.text = $"{AssignedMerc.Name} Lvl. <color=orange>{AssignedMerc.CurrentLevel}</color>";
        }
    }
}