using SRC.Mercs.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SRC.Mercs.UI
{
    public class MercPassiveSlot : MonoBehaviour
    {
        [Header("Text Elements")]
        [SerializeField] private TMP_Text LevelText;
        [SerializeField] private TMP_Text BonusText;
        [Space]
        [SerializeField] private Image IconImage;

        public void Initialize(MercPassive passive)
        {
            LevelText.text = $"Lv {passive.UnlockLevel}";
            BonusText.text = $"<color=orange>{Format.Number(passive.BonusType, passive.BonusValue)}</color> {Format.BonusType(passive.BonusType)}";
        }
    }
}
