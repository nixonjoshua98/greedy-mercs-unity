using GM.Enums;
using GM.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace GM.UI
{
    public class GenericGradeItem : GM.Core.GMMonoBehaviour
    {
        [Header("Defaults")]
        [SerializeField, Tooltip("Optional")] Sprite DefaultIcon;
        [SerializeField] Sprite DefaultBackground;
        [SerializeField] Color DefaultBorderColor;

        [Header("Elements")]
        [SerializeField] Image BackgroundImage;
        [SerializeField] Image IconImage;
        [SerializeField] Image BorderImage;

        public void Empty()
        {
            BackgroundImage.sprite  = DefaultBackground;
            IconImage.sprite        = DefaultIcon;
            BorderImage.color       = DefaultBorderColor;

            if (DefaultIcon == null)
            {
                IconImage.color = new(IconImage.color.r, IconImage.color.g, IconImage.color.g, 0);
            }
        }

        public void Intialize(GM.Mercs.Data.AggregatedMercData merc)
        {
            IconImage.sprite = merc.Icon;

            SetBasicUI(merc.ItemGrade);
        }

        public void Intialize(ItemGrade grade, Sprite icon)
        {
            var config = App.Local.GetItemGrade(grade);

            SetBasicUI(config);

            IconImage.sprite = icon;
        }

        public void Intialize(GM.Artefacts.Data.AggregatedArtefactData artefact)
        {
            IconImage.sprite = artefact.Icon;

            SetBasicUI(artefact.Grade);
        }

        void SetBasicUI(ItemGradeData grade)
        {
            BackgroundImage.sprite = grade.BackgroundSprite;
            BorderImage.color = grade.FrameColour;
        }
    }
}
