using System.Collections;
using UnityEngine;

namespace SRC
{
    public class ServerSyncManager : SRC.Core.GMMonoBehaviour
    {
        private bool isUpdatingQuests;
        private bool isUpdatingBountyShop;

        private void Start()
        {
            StartCoroutine(_UpdateLoop());
        }

        private IEnumerator _UpdateLoop()
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(1);

                if (!isUpdatingBountyShop && !App.BountyShop.IsValid)
                    FetchBountyShop();

                if (!isUpdatingQuests && !App.Quests.IsDailyQuestsValid)
                {
                    FetchQuests();
                }
            }
        }

        private void FetchBountyShop()
        {
            isUpdatingBountyShop = true;

            App.BountyShop.FetchShop(() =>
            {
                if (!App.BountyShop.IsValid)
                {
                    Invoke("FetchBountyShop", 1.0f);
                }

                isUpdatingBountyShop = !App.BountyShop.IsValid;
            });
        }

        private void FetchQuests()
        {
            isUpdatingQuests = true;

            App.Quests.FetchQuests(success =>
            {
                if (!App.Quests.IsDailyQuestsValid)
                {
                    Invoke("FetchQuests", 1.0f);
                }

                isUpdatingQuests = !App.Quests.IsDailyQuestsValid;
            });
        }
    }
}
