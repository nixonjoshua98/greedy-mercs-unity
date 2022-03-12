using GM.Common.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GM.Mercs.UI
{
    public class MercDamageSummarySlot : Core.GMMonoBehaviour
    {
        [SerializeField] Sprite DefaultIcon;
        [Space]
        [SerializeField] Image MercIconImage;
        [SerializeField] TMP_Text DamagePercentText;
        [SerializeField] Slider DamageSlider;
        [SerializeField] TMP_Text DamageText;

        public void SetEmpty()
        {
            MercIconImage.sprite = DefaultIcon;

            DamagePercentText.text = "0%";
            DamageText.text = "0";
            DamageSlider.value = 0;
        }

        public void UpdateValues(MercID mercId, BigDouble val, float percent)
        {
            MercIconImage.sprite = App.DataContainers.Mercs.GetGameMerc(mercId).Icon;

            DamagePercentText.text = Format.Percentage(percent);
            DamageText.text = Format.Number(val);
            DamageSlider.value = percent;
        }
    }
}
