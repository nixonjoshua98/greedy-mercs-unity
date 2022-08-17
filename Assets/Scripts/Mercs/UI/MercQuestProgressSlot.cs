using SRC.Common;
using SRC.UI;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SRC.Mercs.UI
{
    public class MercQuestProgressSlot : SRC.Core.GMMonoBehaviour
    {
        [Header("Text Elements")]
        [SerializeField] private TMP_Text TitleText;
        [SerializeField] private TMP_Text DamageText;
        [Space]
        [SerializeField] private GenericGradeItem GradeSlot;
        [SerializeField] private Button CompleteQuestButton;
        [SerializeField] private Image ButtonBackgroundImage;
        [SerializeField] private Slider ProgressSlider;

        private SRC.Quests.AggregatedMercQuest MercQuest => App.Quests.MercQuests.Where(x => !x.IsCompleted).OrderByDescending(x => x.CurrentProgress).ThenBy(x => x.ID).FirstOrDefault();

        private void Awake()
        {
            if (MercQuest is null)
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            InvokeRepeating(nameof(UpdateUI), 0.0f, 0.5f);
        }

        private void UpdateUI()
        {
            var quest = MercQuest;

            var merc = App.Mercs.GetMerc(quest.RewardMercID);

            TitleText.text = quest.Title;
            ProgressSlider.value = quest.CurrentProgress;
            DamageText.text = $"<color=orange>{Format.Number(merc.BaseDamage)}</color> DMG";

            CompleteQuestButton.interactable = quest.CurrentProgress >= 1.0f;
            ButtonBackgroundImage.color = quest.CurrentProgress >= 1.0f ? Constants.Colors.SoftGreen : Constants.Colors.Grey;

            GradeSlot.Intialize(App.Mercs.GetMerc(quest.RewardMercID));
        }

        /* Callbacks */

        public void OnCompleteQuestButton()
        {
            if (MercQuest.CurrentProgress >= 1.0f)
            {
                App.Quests.CompleteQuest(MercQuest, (success) =>
                {
                    if (MercQuest is null)
                    {
                        Destroy(gameObject);
                    }
                    else
                    {
                        UpdateUI();
                    }
                });
            }
        }
    }
}
