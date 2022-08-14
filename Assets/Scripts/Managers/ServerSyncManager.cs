using System.Collections;
using UnityEngine;

namespace GM
{
    public class ServerSyncManager : GM.Core.GMMonoBehaviour
    {
        private bool isUpdatingQuests;

        bool isUpdatingBountyShop;

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

        void FetchBountyShop()
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
