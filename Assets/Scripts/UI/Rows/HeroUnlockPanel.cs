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
            if (StaticData.CharacterList.GetNextHero(out CharacterID chara))
            {
                MercData mercData = StaticData.Mercs.GetMerc(chara);

                CostText.text = Utils.Format.FormatNumber(mercData.UnlockCost);
            }
        }

        // === Button Callbacks ===

        public void OnUnlockButton()
        {
            if (StaticData.CharacterList.GetNextHero(out CharacterID chara))
            {
                MercData mercData = StaticData.Mercs.GetMerc(chara);

                if (GameState.Player.gold >= mercData.UnlockCost)
                {
                    GameState.Player.gold -= mercData.UnlockCost;

                    GameState.Characters.Add(chara);

                    Events.OnCharacterUnlocked.Invoke(chara);
                }

                UpdatePanel();
            }
        }
    }
}