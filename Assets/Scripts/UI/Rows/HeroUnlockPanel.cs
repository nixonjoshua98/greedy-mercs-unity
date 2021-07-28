using UnityEngine;
using UnityEngine.UI;

namespace GM
{
    using GM.Data;
    using GM.Units;
    using GM.Events;

    public class HeroUnlockPanel : MonoBehaviour
    {
        [SerializeField] Text CostText;

        void Start()
        {
            for (int i = 0; i < 5; ++i)
            {
                if (MercenaryManager.Instance.GetNextHero(out MercID chara))
                {
                    MercenaryManager.Instance.SetState(chara);

                    GlobalEvents.E_OnMercUnlocked.Invoke(chara);
                }
            }


            UpdatePanel();
        }

        void UpdatePanel()
        {
            CostText.text = "-";

            if (MercenaryManager.Instance.GetNextHero(out MercID chara))
            {
                MercData mercData = GameData.Get().Mercs.Get(chara);

                CostText.text = FormatString.Number(mercData.UnlockCost);
            }
        }

        // === Button Callbacks ===

        public void OnUnlockButton()
        {
            if (MercenaryManager.Instance.GetNextHero(out MercID chara))
            {
                MercData mercData = GameData.Get().Mercs.Get(chara);

                if (GameState.Player.gold >= mercData.UnlockCost)
                {
                    GameState.Player.gold -= mercData.UnlockCost;

                    MercenaryManager.Instance.SetState(chara);

                    GlobalEvents.E_OnMercUnlocked.Invoke(chara);
                }

                UpdatePanel();
            }
        }
    }
}