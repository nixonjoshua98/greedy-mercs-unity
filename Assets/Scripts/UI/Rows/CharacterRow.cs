using UnityEngine;
using UnityEngine.UI;

namespace GreedyMercs
{
    public class CharacterRow : MonoBehaviour
    {
        [Header("Images")]
        [SerializeField] Image Icon;

        [Header("Text")]
        [SerializeField] Text nameText;
        [SerializeField] Text DamageText;
        [SerializeField] Text UpgradeCost;

        [Header("Buttons")]
        [SerializeField] Button UpgradeButton;

        [Header("Prefabs")]
        [SerializeField] GameObject CharacterPanelObject;

        CharacterSO character;

        protected int BuyAmount
        {
            get
            {
                if (MercsTab.BuyAmount == -1)
                    return Formulas.AffordCharacterLevels(character.CharacterID);

                var state = GameState.Characters.Get(character.CharacterID);

                return Mathf.Min(MercsTab.BuyAmount, StaticData.MAX_CHAR_LEVEL - state.level);
            }
        }

        public void SetCharacter(CharacterSO chara)
        {
            character = chara;

            nameText.text = chara.name;
            Icon.sprite = chara.icon;

            UpdateRow();

            InvokeRepeating("UpdateRow", 0.25f, 0.25f);
        }

        void UpdateRow()
        {
            var state = GameState.Characters.Get(character.CharacterID);

            DamageText.text = Utils.Format.FormatNumber(StatsCache.GetCharacterDamage(character.CharacterID)) + " DPS";

            if (state.level < StaticData.MAX_CHAR_LEVEL)
            {
                string cost = Utils.Format.FormatNumber(Formulas.CalcCharacterLevelUpCost(character.CharacterID, BuyAmount));

                UpgradeCost.text = string.Format("x{0}\n{1}", BuyAmount, cost);
            }

            else
                UpgradeCost.text = "MAX";

            nameText.text = string.Format("(Lvl. {0}) {1}", state.level, character.name);

            UpgradeButton.interactable = state.level < StaticData.MAX_CHAR_LEVEL;
        }

        // === Button Callbacks ===

        public void OnUpgrade()
        {
            int levelsBuying = BuyAmount;

            var state = GameState.Characters.Get(character.CharacterID);

            BigDouble cost = Formulas.CalcCharacterLevelUpCost(character.CharacterID, levelsBuying);

            if (state.level + levelsBuying <= StaticData.MAX_CHAR_LEVEL && GameState.Player.gold >= cost)
            {
                state.level += levelsBuying;

                GameState.Player.gold -= cost;

                UpdateRow();

                Events.OnCharacterLevelUp.Invoke(character.CharacterID);
            }
        }

        public void OnShowInfo()
        {
            GameObject panel = Utils.UI.Instantiate(CharacterPanelObject, Vector3.zero);

            panel.GetComponent<CharacterPanel>().SetHero(character.CharacterID);
        }
    }
}