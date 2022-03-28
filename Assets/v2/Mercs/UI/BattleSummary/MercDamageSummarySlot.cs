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
        [SerializeField] Slider DamageSlider;
        [SerializeField] TMP_Text DamageText;
        [SerializeField] TMP_Text NameText;

        public void SetEmpty()
        {
            MercIconImage.sprite = DefaultIcon;

            NameText.text = "";
            DamageText.text = "0.0K (0.0%)";
            DamageSlider.value = 0;
        }

        public void UpdateValues(MercID mercId, BigDouble val, float percent)
        {
            var mercData = App.Mercs.GetGameMerc(mercId);

            MercIconImage.sprite = mercData.Icon;
            NameText.text = mercData.Name;

            DamageText.text = $"{Format.Number(val)} ({Format.Percentage(percent)})";

            DamageSlider.value = percent;
        }
    }
}
