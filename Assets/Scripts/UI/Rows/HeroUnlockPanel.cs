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
                GM.Mercs.Data.MercGameData mercData = App.Data.Mercs.Game[chara];

                CostText.text = FormatString.Number(mercData.UnlockCost);
            }
        }

        // === Button Callbacks ===

        public void OnUnlockButton()
        {
            if (App.Data.Mercs.GetNextHero(out MercID chara))
            {
                GM.Mercs.Data.MercGameData mercData = App.Data.Mercs.Game[chara];

                if (App.Data.Inv.Gold >= mercData.UnlockCost)
                {
                    App.Data.Inv.Gold -= mercData.UnlockCost;

                    // Unlock the merc
                    App.Data.Mercs.User[chara] = new Mercs.Data.MercUserData
                    {
                        Level = 1
                    };

                    GlobalEvents.E_OnMercUnlocked.Invoke(chara);
                }

                UpdatePanel();
            }
        }
    }
}