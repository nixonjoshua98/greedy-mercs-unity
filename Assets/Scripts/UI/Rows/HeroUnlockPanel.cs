using UnityEngine;
using UnityEngine.UI;

namespace GM
{
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

            if (App.Data.Mercs.GetNextHero(out MercID chara))
            {
                GM.Mercs.Models.MercGameDataModel mercData = App.Data.Mercs.GetGameMerc(chara);

                CostText.text = Format.Number(mercData.UnlockCost);
            }
        }

        // === Button Callbacks ===

        public void OnUnlockButton()
        {
            if (App.Data.Mercs.GetNextHero(out MercID chara))
            {
                GM.Mercs.Models.MercGameDataModel mercData = App.Data.Mercs.GetGameMerc(chara);

                if (App.Data.Inv.Gold >= mercData.UnlockCost)
                {
                    App.Data.Inv.Gold -= mercData.UnlockCost;

                    App.Data.Mercs.UnlockUserMerc(chara);

                    GlobalEvents.E_OnMercUnlocked.Invoke(chara);
                }

                UpdatePanel();
            }
        }
    }
}