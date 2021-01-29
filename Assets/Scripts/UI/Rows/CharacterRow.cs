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

            UpdateUI();

            MercsTab.AddBuyAmountListener(UpdateUI);
        }

        void OnEnable() => UpdateUI();
        void OnDisable() => CancelInvoke("UpdateUI");

        void UpdateUI() => UpdateUI(MercsTab.BuyAmount);
        void UpdateUI(int amount)
        {
            // A character may not be set yet, since the row is dynamic
            if (character == null)
                return;

            // Loop the function is it is MAX since gold constantly changes
            if (amount == -1 && !IsInvoking("UpdateUI"))
                InvokeRepeating("UpdateUI", 0.5f, 0.5f);

            else if (amount != -1) // Cancel the invoke if the value is a flatvalue (1, 10 etc)
                CancelInvoke("UpdateUI");


            var state = GameState.Characters.Get(character.CharacterID);

            DamageText.text = Utils.Format.FormatNumber(StatsCache.GetCharacterDamage(character.CharacterID)) + " DPS";
            nameText.text   = string.Format("(Lvl. {0}) {1}", state.level, character.name);

            if (state.level < StaticData.MAX_CHAR_LEVEL)
            {
                BigDouble cost = Formulas.CalcCharacterLevelUpCost(character.CharacterID, BuyAmount);

                UpgradeCost.text            = string.Format("x{0}\n{1}", BuyAmount, Utils.Format.FormatNumber(cost));
                UpgradeButton.interactable  = GameState.Player.gold >= cost;
            }

            else
            {
                UpgradeCost.text            = "MAX";
                UpgradeButton.interactable  = false;
            }

            UpgradeButton.interactable = UpgradeButton.interactable && BuyAmount > 0;
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

                Events.OnCharacterLevelUp.Invoke(character.CharacterID);
            }

            UpdateUI();
        }

        public void OnShowInfo()
        {
            GameObject panel = Utils.UI.Instantiate(CharacterPanelObject, Vector3.zero);

            panel.GetComponent<CharacterPanel>().SetHero(character.CharacterID);
        }
    }
}