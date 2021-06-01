using UnityEngine;
using UnityEngine.UI;

namespace GreedyMercs
{
    using GM.Characters;

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

                CostText.text = Utils.Format.FormatNumber(mercData.UnlockCost);
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

                    Events.OnCharacterUnlocked.Invoke(chara);
                }

                UpdatePanel();
            }
        }
    }
}