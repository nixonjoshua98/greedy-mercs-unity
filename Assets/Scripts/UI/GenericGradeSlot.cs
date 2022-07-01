using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
            BackgroundImage.sprite = merc.ItemGrade.BackgroundSprite;
            BorderImage.color = merc.ItemGrade.FrameColour;
        }
    }
}
