using System.Collections;
using UnityEngine;

namespace SRC.UI.IconButtons
{
    public class QuestsPopupButton : SRC.Core.GMMonoBehaviour
    {
        [SerializeField] private PillBadge Pill;

        private void Awake()
        {
            StartCoroutine(_InternalLoop());
        }

        private IEnumerator _InternalLoop()
        {
            while (true)
            {
                int numQuests = App.Quests.NumQuestsReadyToClaim;

                if (numQuests == 0)
                    Pill.Hide();

                else
                    Pill.Show(numQuests);

                yield return new WaitForSecondsRealtime(0.5f);
            }
        }
    }
}
