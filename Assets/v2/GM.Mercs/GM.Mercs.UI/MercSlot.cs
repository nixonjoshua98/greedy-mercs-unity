using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GM.Mercs.UI
{
    public class MercSlot : MercUIObject
    {
        [Header("References")]
        public Image IconImage;
        public TMP_Text NameText;
        public TMP_Text LevelText;
        [Space]
        public GM.UI.VStackedButton Button;

        int _buyAmount;
        protected int buyAmount
        {
            get
            {
                if (_buyAmount == -1)
                    return Formulas.AffordCharacterLevels(AssignedMerc.Id);

                return Mathf.Min(_buyAmount, global::Constants.MAX_CHAR_LEVEL - AssignedMerc.CurrentLevel);
            }
        }

        protected override void OnAssigned()
        {
            IconImage.sprite = AssignedMerc.Icon;
            NameText.text = AssignedMerc.Name;
        }

        void FixedUpdate()
        {
            LevelText.text = $"Level {AssignedMerc.CurrentLevel}";

            Button.SetText("", "");

            if (!AssignedMerc.IsMaxLevel)
            {
                Button.SetText($"x{buyAmount}", Format.Number(Formulas.MercLevelUpCost(AssignedMerc.CurrentLevel, buyAmount, AssignedMerc.UnlockCost)));
            }
        }
    }
}