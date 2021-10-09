using TMPro;
using UnityEngine;
using UnityEngine.UI;
using BigInteger = System.Numerics.BigInteger;

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
        [Space]
        public UI_.VStackedButton UpgradeButton;

        public void AssignArtefact(int artefactId)
        {
            AssignedArtefactId = artefactId;

            InitialSetup();
        }

        void InitialSetup()
        {
            NameText.text = AssignedArtefact.Name;
            IconImage.sprite = AssignedArtefact.Icon;

            SetUpgradeRelatedText();
        }

        void SetUpgradeRelatedText()
        {
            UpgradeButton.SetText("Max Level", "-");

            if (!AssignedArtefact.IsMaxLevel)
            {
                BigInteger ugradeCost = App.Cache.ArtefactUpgradeCost(AssignedArtefact, 1);

                UpgradeButton.SetText("x1", FormatString.Number(ugradeCost));
            }

            LevelText.text = $"Level {AssignedArtefact.CurrentLevel}";
            BonusText.text = FormatString.Bonus(AssignedArtefact.Bonus, AssignedArtefact.BaseEffect);
        }


        // == Callbacks == //
        public void OnUpgradeButton()
        {
            App.Data.Arts.UpgradeArtefact(AssignedArtefactId, 1, (success) =>
            {
                if (success)
                {
                    SetUpgradeRelatedText();
                }
            });
        }
    }
}