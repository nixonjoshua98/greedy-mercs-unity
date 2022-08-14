using GM.Mercs.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GM.Mercs.UI
{
    public class MercPassiveSlot : MonoBehaviour
    {
        [Header("Text Elements")]
        [SerializeField] TMP_Text LevelText;
        [SerializeField] TMP_Text BonusText;
        [Space]
        [SerializeField] Image IconImage;

        public void Initialize(MercPassive passive)
        {
            LevelText.text = $"Lv {passive.UnlockLevel}";
            BonusText.text = $"<color=orange>{Format.Number(passive.BonusType, passive.BonusValue)}</color> {Format.BonusType(passive.BonusType)}";
        }
    }
}
