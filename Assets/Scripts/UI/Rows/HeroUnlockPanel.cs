using UnityEngine;
using UnityEngine.UI;

namespace GM
{
    using GM.Characters;
    using GM.Events;

    public class HeroUnlockPanel : MonoBehaviour
    {
        [SerializeField] Text CostText;

        void Start()
        {
            UpdatePanel();
        }

        void UpdatePanel()
        {
            CostText.text = "-";

            if (MercenaryManager.Instance.GetNextHero(out CharacterID chara))
            {
                MercData mercData = StaticData.Mercs.GetMerc(chara);

                CostText.text = FormatString.Number(mercData.UnlockCost);
            }
        }

        // === Button Callbacks ===

        public void OnUnlockButton()
        {
            if (MercenaryManager.Instance.GetNextHero(out CharacterID chara))
            {
                MercData mercData = StaticData.Mercs.GetMerc(chara);

                if (GameState.Player.gold >= mercData.UnlockCost)
                {
                    GameState.Player.gold -= mercData.UnlockCost;

                    MercenaryManager.Instance.SetState(chara);

                    GlobalEvents.OnCharacterUnlocked.Invoke(chara);
                }

                UpdatePanel();
            }
        }
    }
}