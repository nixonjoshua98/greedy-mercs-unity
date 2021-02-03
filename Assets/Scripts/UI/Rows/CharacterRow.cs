using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GreedyMercs
{
    [System.Serializable]
    struct PassiveUnlockObject
    {
        public GameObject root;

        public Text text;
    }

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

        [Space]
        [SerializeField] PassiveUnlockObject passiveUnlockObject;

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
        }

        void OnEnable() => InvokeRepeating("UpdateUI", 0.0f, 0.5f);
        void OnDisable() => CancelInvoke("UpdateUI");
        
        void UpdateUI()
        {
            // A character may not be set yet, since the row is dynamic
            if (character == null) return;

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

            UpgradeState state  = GameState.Characters.Get(character.CharacterID);
            BigDouble cost      = Formulas.CalcCharacterLevelUpCost(character.CharacterID, levelsBuying);

            int numPassives = GameState.Characters.GetUnlockedPassives(character.CharacterID).Count;

            if (state.level + levelsBuying <= StaticData.MAX_CHAR_LEVEL && GameState.Player.gold >= cost)
            {
                state.level += levelsBuying;

                GameState.Player.gold -= cost;

                Events.OnCharacterLevelUp.Invoke(character.CharacterID);
            }

            List<CharacterPassive> passives = GameState.Characters.GetUnlockedPassives(character.CharacterID);

            if (passives.Count > numPassives)
                ShowUnlockedPassive(passives[passives.Count - 1]);

            UpdateUI();
        }

        public void OnShowInfo()
        {
            GameObject panel = Utils.UI.Instantiate(CharacterPanelObject, Vector3.zero);

            panel.GetComponent<CharacterPanel>().SetHero(character.CharacterID);
        }

        void ShowUnlockedPassive(CharacterPassive passive)
        {
            IEnumerator Animation()
            {
                passiveUnlockObject.root.SetActive(true);

                passiveUnlockObject.text.text = passive.ToString();

                yield return new WaitForSeconds(2.0f);

                passiveUnlockObject.root.SetActive(false);
            }

            PersistentMono.Inst.StartCoroutine(Animation());
        }
    }
}