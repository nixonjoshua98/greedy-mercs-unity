using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GM.Mercs.UI
{
    [System.Serializable]
    struct IconBackgroundSprites
    {
        public Sprite UnlockedSprite;
        public Sprite LockedSprite;
    }

    public class MercPassiveSlot : MonoBehaviour
    {
        [SerializeField] IconBackgroundSprites BackgroundSprites;

        [Header("References")]
        public TMP_Text UnlockLevelText;
        public Image IconBackgroundImage;
        public TMP_Text BonusText;

        MercPassiveReference AssignedPassive;

        public void Assign(MercPassiveReference passive, bool isUnlocked)
        {
            AssignedPassive = passive;

            Toggle(isUnlocked);
        }

        void Toggle(bool isUnlocked)
        {
            UnlockLevelText.text = $"Level <color=orange>{AssignedPassive.UnlockLevel}</color>";
            IconBackgroundImage.sprite = isUnlocked ? BackgroundSprites.UnlockedSprite : BackgroundSprites.LockedSprite;
            BonusText.text = Format.Bonus(AssignedPassive.Values.Type, AssignedPassive.Values.Value);
            BonusText.text = $"<color=orange>{Format.Number(AssignedPassive.Values.Value, AssignedPassive.Values.Type)}</color> {Format.Bonus(AssignedPassive.Values.Type)}";
        }
    }
}
