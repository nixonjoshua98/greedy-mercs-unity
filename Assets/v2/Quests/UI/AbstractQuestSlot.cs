using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GM.Quests.UI
{
    [System.Serializable]
    public class ClaimButtonColours
    {
        public Color InProgress = GM.Common.Constants.Colors.Grey;
        public Color ReadyToComplete = GM.Common.Constants.Colors.Green;
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
