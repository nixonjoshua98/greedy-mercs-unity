using TMPro;
using UnityEngine;

namespace GM.Mercs.UI
{
    public class MercPopup : MercUIObject
    {
        [Header("Prefabs")]
        public GameObject PassiveSlotObject;

        [Header("References")]
        public TMP_Text NameText;
        public Transform PassivesParent;

        protected override void OnAssigned()
        {
            foreach (MercPassiveReference p in AssignedMerc.Passives)
            {
                Instantiate<MercPassiveSlot>(PassiveSlotObject, PassivesParent).Assign(p, AssignedMerc.IsPassiveUnlocked(p));
            }

            SetTitle();
        }

        void SetTitle()
        {
            NameText.text = $"{AssignedMerc.Name} Lvl. <color=orange>{AssignedMerc.CurrentLevel}</color>";
        }
    }
}