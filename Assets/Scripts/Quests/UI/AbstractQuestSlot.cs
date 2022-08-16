using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SRC.Quests.UI
{
    [System.Serializable]
    public class ClaimButtonColours
    {
        public Color InProgress = SRC.Common.Constants.Colors.Grey;
        public Color ReadyToComplete = SRC.Common.Constants.Colors.SoftGreen;
    }

    public abstract class AbstractQuestSlot : MonoBehaviour
    {
        [SerializeField] protected ClaimButtonColours ButtonColours;
        [Space]
        [SerializeField] protected TMP_Text Title;
        [SerializeField] protected Slider ProgressSlider;
        [SerializeField] protected Button CompleteButton;
        [SerializeField] protected Image CompleteButtonImage;
        [SerializeField] protected GameObject CompletedOverlay;
    }
}
