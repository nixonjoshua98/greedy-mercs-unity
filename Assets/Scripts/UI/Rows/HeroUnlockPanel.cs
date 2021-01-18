using UnityEngine;
using UnityEngine.UI;

namespace UI.Characters
{
    using Data.Characters;

    public class HeroUnlockPanel : MonoBehaviour
    {
        [SerializeField] Text TitleText;
        [SerializeField] Text CostText;

        void Start()
        {
            UpdatePanel();
        }

        void UpdatePanel()
        {
            if (StaticData.CharacterList.GetNextHero(out CharacterSO chara))
            {
                TitleText.text = chara.name;

                CostText.text = Utils.Format.FormatNumber(chara.purchaseCost);
            }

            else
                Destroy(gameObject);
        }

        // === Button Callbacks ===

        public void OnUnlockButton()
        {
            if (StaticData.CharacterList.GetNextHero(out CharacterSO chara))
            {
                if (GameState.Player.gold >= chara.purchaseCost)
                {
                    GameState.Player.gold -= chara.purchaseCost;

                    GameState.Characters.Add(chara.CharacterID);

                    Events.OnCharacterUnlocked.Invoke(chara.CharacterID);
                }

                UpdatePanel();
            }
        }
    }
}