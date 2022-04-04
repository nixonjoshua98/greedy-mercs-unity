using TMPro;
using UnityEngine;

namespace GM.Mercs.UI
{
    [System.Serializable]
    internal struct IconBackgroundSprites
    {
        public Sprite UnlockedSprite;
        public Sprite LockedSprite;
    }

    public class MercPassiveSlot : MonoBehaviour
    {
        [SerializeField] private IconBackgroundSprites BackgroundSprites;

        [Header("References")]
        public TMP_Text UnlockLevelText;
        public TMP_Text BonusText;
        private MercPassiveReference AssignedPassive;

        public void Assign(MercPassiveReference passive, bool isUnlocked)
        {
            AssignedPassive = passive;

            Toggle(isUnlocked);
        }

        private void Toggle(bool isUnlocked)
        {
            UnlockLevelText.text = $"Level <color=orange>{AssignedPassive.UnlockLevel}</color>";
            BonusText.text = Format.Bonus(AssignedPassive.Values.Type, AssignedPassive.Values.Value);
            BonusText.text = $"<color=orange>{Format.Number(AssignedPassive.Values.Value, AssignedPassive.Values.Type)}</color> {Format.Bonus(AssignedPassive.Values.Type)}";
        }
    }
}
