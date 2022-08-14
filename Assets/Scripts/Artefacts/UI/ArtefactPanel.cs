using TMPro;
using UnityEngine;

namespace GM.Artefacts.UI
{
    public class ArtefactPanel : ArtefactUIObject
    {
        [SerializeField] GM.UI.GenericGradeItem GradeSlot;

        [Header("Text References")]
        [SerializeField] TMP_Text CurrentBonusText;
        [SerializeField] TMP_Text NameText;
        [SerializeField] TMP_Text LevelText;

        protected override void OnIntialize()
        {
            GradeSlot.Intialize(Artefact);

            UpdateUI();
        }

        void UpdateUI()
        {
            CurrentBonusText.text = $"<color=orange>{Format.Number(Artefact.Effect, Artefact.Bonus)}</color> {Format.BonusType(Artefact.Bonus)}";
            NameText.text = Artefact.Name;
            LevelText.text = $"Lv <color=orange>{Artefact.CurrentLevel}</color>";
        }

        public void OnCloseButton()
        {
            Destroy(gameObject);
        }
    }
}