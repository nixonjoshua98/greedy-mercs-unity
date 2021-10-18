using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GM.Artefacts.UI
{
    public class ArtefactPopup : ArtefactUIObject
    {
        [Header("UI References")]
        public Image IconImage;
        [Space]
        public TMP_Text NameText;
        public TMP_Text CurrentBonusText;

        public override void AssignArtefact(int artefactId)
        {
            base.AssignArtefact(artefactId);

            SetUIElements();
        }

        void SetUIElements()
        {
            IconImage.sprite = AssignedArtefact.Icon;

            NameText.text = AssignedArtefact.Name;
            CurrentBonusText.text = FormatString.Bonus(AssignedArtefact.Bonus, AssignedArtefact.BaseEffect);
        }
    }
}
