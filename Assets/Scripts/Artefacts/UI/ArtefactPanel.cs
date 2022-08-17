using TMPro;
using UnityEngine;

namespace SRC.Artefacts.UI
{
    public class ArtefactPanel : ArtefactUIObject
    {
        [SerializeField] private SRC.UI.GenericGradeItem GradeSlot;

        [Header("Text References")]
        [SerializeField] private TMP_Text CurrentBonusText;
        [SerializeField] private TMP_Text NameText;
        [SerializeField] private TMP_Text LevelText;

        protected override void OnIntialize()
        {
            GradeSlot.Intialize(Artefact);

            UpdateUI();
        }

        private void UpdateUI()
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