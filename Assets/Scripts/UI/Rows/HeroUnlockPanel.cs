using UnityEngine;
using UnityEngine.UI;

namespace GM
{
    using GM.Data;
    using GM.Units;
    using GM.Events;

    public class HeroUnlockPanel : Core.GMMonoBehaviour
    {
        [SerializeField] Text CostText;

        void Start()
        {
            UpdatePanel();
        }


        void UpdatePanel()
        {
            CostText.text = "-";

            if (MercenaryManager.Instance.GetNextHero(out MercID chara))
            {
                GM.Mercs.Data.FullMercData mercData = App.Data.Mercs.GetMerc(chara);

                CostText.text = FormatString.Number(mercData.GameValues.UnlockCost);
            }
        }

        // === Button Callbacks ===

        public void OnUnlockButton()
        {
            if (MercenaryManager.Instance.GetNextHero(out MercID chara))
            {
                GM.Mercs.Data.FullMercData mercData = App.Data.Mercs.GetMerc(chara);

                if (UserData.Get.Inventory.Gold >= mercData.GameValues.UnlockCost)
                {
                    UserData.Get.Inventory.Gold -= mercData.GameValues.UnlockCost;

                    MercenaryManager.Instance.SetState(chara);

                    GlobalEvents.E_OnMercUnlocked.Invoke(chara);
                }

                UpdatePanel();
            }
        }
    }
}