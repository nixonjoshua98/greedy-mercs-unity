using TMPro;
using UnityEngine;

namespace GM.Artefacts.UI
{
    public class ArtefactPopup : ArtefactUIObject
    {
        [Header("UI References")]
        public ArtefactIcon Icon;
        [Space]
        public TMP_Text CurrentBonusText;

        protected override void OnAssigned()
        {
            Icon.Set(AssignedArtefact);

            CurrentBonusText.text = GetBonusText();
        }
    }
}