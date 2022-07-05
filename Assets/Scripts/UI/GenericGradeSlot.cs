using GM.Common.Enums;
using GM.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace GM.UI
{
    public class GenericGradeSlot : GM.Core.GMMonoBehaviour
    {
        [SerializeField] Image BackgroundImage;
        [SerializeField] Image IconImage;
        [SerializeField] Image BorderImage;

        public void Intialize(GM.Mercs.Data.AggregatedMercData merc)
        {
            IconImage.sprite = merc.Icon;

            SetBasicUI(merc.ItemGrade);
        }

        public void Intialize(ItemGradeID grade, Sprite icon)
        {
            var config = App.Local.GetItemGrade(grade);

            SetBasicUI(config);

            IconImage.sprite = icon;
        }

        void SetBasicUI(ItemGradeData grade)
        {
            BackgroundImage.sprite = grade.BackgroundSprite;
            BorderImage.color = grade.FrameColour;
        }
    }
}
