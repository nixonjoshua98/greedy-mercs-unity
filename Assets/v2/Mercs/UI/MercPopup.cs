using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GM.Mercs.UI
{
    public class MercPopup : MercUIObject
    {
        [Header("Prefabs")]
        public GameObject PassiveSlotObject;

        [Header("Components")]
        public TMP_Text NameText;
        public TMP_Text EnergyText;
        public Image IconImage;

        [Header("References")]
        public Transform PassivesParent;

        protected override void OnAssigned()
        {
            foreach (MercPassiveReference p in AssignedMerc.Passives)
            {
                Instantiate<MercPassiveSlot>(PassiveSlotObject, PassivesParent).Assign(p, AssignedMerc.IsPassiveUnlocked(p));
            }

            SetUI();
        }

        private void SetUI()
        {
            NameText.text = $"{AssignedMerc.Name} Lvl. <color=orange>{AssignedMerc.CurrentLevel}</color>";
            EnergyText.text = AssignedMerc.SpawnEnergyRequired.ToString();
            IconImage.sprite = AssignedMerc.Icon;
        }
    }
}