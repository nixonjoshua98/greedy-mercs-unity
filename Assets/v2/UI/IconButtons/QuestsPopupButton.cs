using System.Collections;
using UnityEngine;

namespace GM.UI.IconButtons
{
    public class QuestsPopupButton : GM.Core.GMMonoBehaviour
    {
        [SerializeField] PillBadge Pill;

        void Awake()
        {
            StartCoroutine(_InternalLoop());
        }

        IEnumerator _InternalLoop()
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
