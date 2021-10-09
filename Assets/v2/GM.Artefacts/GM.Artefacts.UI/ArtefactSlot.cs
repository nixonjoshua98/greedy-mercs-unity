using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace GM.Artefacts.UI
{
    public class ArtefactSlot : ArtefactUIObject
    {
        [Header("References")]
        public Image IconImage;
        [Space]
        public TMP_Text NameText;
        public TMP_Text LevelText;
        public TMP_Text BonusText;

        public void AssignArtefact(int artefactId)
        {
            AssignedArtefactId = artefactId;

            SetStaticElements();
        }

        void SetStaticElements()
        {
            NameText.text = AssignedArtefact.Name;
            IconImage.sprite = AssignedArtefact.Icon;
        }

        void FixedUpdate()
        {
            if (AssignedArtefactId != -1)
            {
                OnFixedUpdateWithArtefact();
            }
        }

        void OnFixedUpdateWithArtefact()
        {
            LevelText.text = $"Level {AssignedArtefact.CurrentLevel}";
            BonusText.text = FormatString.Bonus(AssignedArtefact.Bonus, AssignedArtefact.BaseEffect);
        }


        // == Callbacks == //
        public void OnUpgradeButton()
        {
            App.Data.Arts.UpgradeArtefact(AssignedArtefactId, 1, (success) =>
            {

            });
        }
    }
}