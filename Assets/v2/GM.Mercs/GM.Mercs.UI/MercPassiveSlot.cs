using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

        GM.Mercs.Models.MercPassiveDataModel AssignedPassive;

        public void Assign(GM.Mercs.Models.MercPassiveDataModel passive, bool isUnlocked)
        {
            AssignedPassive = passive;

            Toggle(isUnlocked);
        }

        void Toggle(bool isUnlocked)
        {
            UnlockLevelText.text = AssignedPassive.UnlockLevel.ToString();
            IconBackgroundImage.sprite = isUnlocked ? BackgroundSprites.UnlockedSprite : BackgroundSprites.LockedSprite;
            BonusText.text = Format.Bonus(AssignedPassive.Type, AssignedPassive.Value);

        }
    }
}
