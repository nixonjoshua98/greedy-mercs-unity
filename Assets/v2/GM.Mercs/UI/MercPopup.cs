using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
            foreach (GM.Mercs.Models.MercPassiveDataModel p in AssignedMerc.Passives)
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