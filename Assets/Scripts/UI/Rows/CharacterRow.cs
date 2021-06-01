using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GM.Characters
{
    using Utils = GreedyMercs.Utils;

    using GreedyMercs;
    using GreedyMercs.UI;

    using GM.UI;

    public class CharacterRow : ExtendedMonoBehaviour
    {
        [SerializeField] StackedButton upgradeButton;

        [Header("Images")]
        [SerializeField] Image Icon;

        [Header("Text")]
        [SerializeField] Text nameText;
        [SerializeField] Text DamageText;

        [Header("Prefabs")]
        [SerializeField] GameObject CharacterPanelObject;

        MercContainer assignedCharacter;

        int _buyAmount;
        bool _updatingUi;

        void Awake()
        {
            BuyController buyController = FindObjectOfType<MercsTab>().GetComponentInChildren<BuyController>();

            buyController.AddListener((val) => { _buyAmount = val; });
        }

        protected int TargetBuyAmount
        {
            get
            {
                if (_buyAmount == -1)
                    return Formulas.AffordCharacterLevels(assignedCharacter.ID);

                var state = GameState.Characters.Get(assignedCharacter.ID);

                return Mathf.Min(_buyAmount, StaticData.MAX_CHAR_LEVEL - state.level);
            }
        }

        protected UpgradeState State { get { return GameState.Characters.Get(assignedCharacter.ID); } }

        public void SetCharacter(MercContainer chara)
        {
            assignedCharacter = chara;

            nameText.text = chara.name;
            Icon.sprite = chara.Icon;

            _updatingUi = true;
        }
        
        protected override void PeriodicUpdate()
        {
            if (!_updatingUi)
                return;

            DamageText.text = Utils.Format.FormatNumber(StatsCache.CharacterDamage(assignedCharacter.ID)) + " DPS";
            nameText.text   = string.Format("(Lvl. {0}) {1}", State.level, assignedCharacter.name);

            upgradeButton.SetText("MAX", "-");

            if (State.level < StaticData.MAX_CHAR_LEVEL)
            {
                BigDouble cost = Formulas.CalcCharacterLevelUpCost(assignedCharacter.ID, TargetBuyAmount);

                upgradeButton.SetText(string.Format("x{0}", TargetBuyAmount), Utils.Format.FormatNumber(cost));
            }
        }

        // === Button Callbacks ===

        public void OnUpgrade()
        {
            int levelsBuying = TargetBuyAmount;

            BigDouble cost = Formulas.CalcCharacterLevelUpCost(assignedCharacter.ID, levelsBuying);

            if (State.level + levelsBuying <= StaticData.MAX_CHAR_LEVEL && GameState.Player.gold >= cost)
            {
                State.level += levelsBuying;

                GameState.Player.gold -= cost;

                Events.OnCharacterLevelUp.Invoke(assignedCharacter.ID);
            }
        }

        public void OnShowInfo()
        {
            GameObject panel = Utils.UI.Instantiate(CharacterPanelObject, Vector3.zero);

            panel.GetComponent<CharacterPanel>().SetHero(assignedCharacter.ID);
        }
    }
}